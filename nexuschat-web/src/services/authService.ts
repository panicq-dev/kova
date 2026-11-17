const API_URL = process.env.NEXT_PUBLIC_API_URL;

export interface UsuarioSignRequest {
    nomeDeUsuario: string;
    email?: string;
    senha: string;
}

export interface JwtResponse {
    token: string;
    refreshToken: string;
}

function extrairMensagemErro(erroData: unknown, status: number): string {
    if (Array.isArray(erroData) && erroData.length > 0) {
        return erroData
            .map((e: { description?: string }) => e.description || JSON.stringify(e))
            .join(" ");
    }

    if (erroData && typeof erroData === "object") {
        const obj = erroData as {
            errors?: Record<string, string[]>;
            mensagem?: string;
            message?: string;
            title?: string;
        };

        if (obj.errors) {
            return Object.values(obj.errors).flat().join(" ");
        }

        if (obj.mensagem) return obj.mensagem;
        if (obj.message) return obj.message;
        if (obj.title) return `${obj.title} (HTTP ${status})`;
    }

    return `Erro HTTP ${status} ao processar a requisição.`;
}

export async function registrarUsuario(dados: UsuarioSignRequest): Promise<void> {
    if (!API_URL) {
        throw new Error("NEXT_PUBLIC_API_URL não está definida no .env.local.");
    }

    let response: Response;

    try {
        response = await fetch(`${API_URL}/api/usuario/registrar`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dados),
        });
    } catch {
        throw new Error(
            `Falha de conexão com ${API_URL}. Verifique se a API C# está rodando e se o certificado HTTPS foi aceito.`
        );
    }

    if (!response.ok) {
        const erroData = await response.json().catch(() => null);
        throw new Error(extrairMensagemErro(erroData, response.status));
    }
}

export async function loginUsuario(
    dados: Omit<UsuarioSignRequest, "email">
): Promise<JwtResponse> {
    if (!API_URL) {
        throw new Error("NEXT_PUBLIC_API_URL não está definida no .env.local.");
    }

    let response: Response;

    try {
        response = await fetch(`${API_URL}/api/usuario/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                ...dados,
                email: "login@nexuschat.local",
            }),
        });
    } catch {
        throw new Error(
            `Falha de conexão com ${API_URL}. Verifique se a API C# está rodando.`
        );
    }

    if (!response.ok) {
        const erroData = await response.json().catch(() => null);
        if (response.status === 401) {
            throw new Error("Usuário ou senha inválidos.");
        }
        throw new Error(extrairMensagemErro(erroData, response.status));
    }

    const tokens: JwtResponse = await response.json();

    localStorage.setItem("nexus_token", tokens.token);
    localStorage.setItem("nexus_refresh_token", tokens.refreshToken);
    localStorage.setItem("nexus_username", dados.nomeDeUsuario);

    return tokens;
}

export async function renovarSessao(): Promise<JwtResponse | null> {
    if (!API_URL) return null;

    const nomeDeUsuario = localStorage.getItem("nexus_username");
    const refreshToken = localStorage.getItem("nexus_refresh_token");

    if (!nomeDeUsuario || !refreshToken) return null;

    try {
        const response = await fetch(`${API_URL}/api/usuario/refresh-token`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ nomeDeUsuario, refreshToken }),
        });

        if (!response.ok) {
            logoutUsuario();
            return null;
        }

        const novosTokens: JwtResponse = await response.json();
        localStorage.setItem("nexus_token", novosTokens.token);
        localStorage.setItem("nexus_refresh_token", novosTokens.refreshToken);

        return novosTokens;
    } catch {
        return null;
    }
}

export function logoutUsuario(): void {
    localStorage.removeItem("nexus_token");
    localStorage.removeItem("nexus_refresh_token");
    localStorage.removeItem("nexus_username");
}

export async function fetchAutenticado(
    endpoint: string,
    options: RequestInit = {}
): Promise<Response> {
    if (!API_URL) {
        throw new Error("NEXT_PUBLIC_API_URL não está definida no .env.local.");
    }

    const token = localStorage.getItem("nexus_token");

    const fazerRequisicao = (accessToken: string | null) =>
        fetch(`${API_URL}${endpoint}`, {
            ...options,
            headers: {
                "Content-Type": "application/json",
                ...(accessToken ? { Authorization: `Bearer ${accessToken}` } : {}),
                ...options.headers,
            },
        });

    let response = await fazerRequisicao(token);

    if (response.status === 401) {
        const renovado = await renovarSessao();
        if (renovado) {
            response = await fazerRequisicao(renovado.token);
        }
    }

    return response;
}