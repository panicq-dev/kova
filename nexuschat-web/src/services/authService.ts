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

export async function registrarUsuario(dados: UsuarioSignRequest): Promise<void> {
    const response = await fetch(`${API_URL}/api/usuario/registrar`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(dados),
    });

    if (!response.ok) {
        const erroData = await response.json().catch(() => null);
        if (Array.isArray(erroData) && erroData.length > 0) {
            throw new Error(erroData.map((e: { description: string }) => e.description).join(" "));
        }
        throw new Error("Não foi possível realizar o cadastro.");
    }
}

export async function loginUsuario(
    dados: Omit<UsuarioSignRequest, "email">
): Promise<JwtResponse> {
    const response = await fetch(`${API_URL}/api/usuario/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(dados),
    });

    if (!response.ok) {
        throw new Error("Usuário ou senha inválidos.");
    }

    const tokens: JwtResponse = await response.json();

    localStorage.setItem("nexus_token", tokens.token);
    localStorage.setItem("nexus_refresh_token", tokens.refreshToken);
    localStorage.setItem("nexus_username", dados.nomeDeUsuario);

    return tokens;
}

export async function renovarSessao(): Promise<JwtResponse | null> {
    const nomeDeUsuario = localStorage.getItem("nexus_username");
    const refreshToken = localStorage.getItem("nexus_refresh_token");

    if (!nomeDeUsuario || !refreshToken) return null;

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
}

export function logoutUsuario(): void {
    localStorage.removeItem("nexus_token");
    localStorage.removeItem("nexus_refresh_token");
    localStorage.removeItem("nexus_username");
}

// Use esta função para qualquer rota protegida com [Authorize] na sua API
export async function fetchAutenticado(
    endpoint: string,
    options: RequestInit = {}
): Promise<Response> {
    let token = localStorage.getItem("nexus_token");

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