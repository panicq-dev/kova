
import { UsuarioResponse } from "@/types/usuario";

const API_URL = process.env.NEXT_PUBLIC_API_URL;

export async function buscarUsuario(nome: string): Promise<UsuarioResponse> {
    let response: Response;

    try {
        console.log("URL da API:", API_URL);
        console.log("Pesquisando usuário:", nome);

        response = await fetch(
            `${API_URL}/api/usuario/buscar-usuario?nomeDeUsuario=${encodeURIComponent(nome)}`,
            {
                method: "GET"
            }
        );
    } catch (error) {
        console.error("Erro na conexão com a API:", error);
        throw new Error("Falha de conexão com a API.");
    }

    if (!response.ok) {
        const mensagem = await response.text();

        console.error("STATUS HTTP:", response.status);
        console.error("MENSAGEM DA API:", mensagem);
        console.error("URL DA REQUISIÇÃO:", response.url);

        throw new Error(`Erro ao buscar usuário: HTTP ${response.status}`);
    }

    const resultado: UsuarioResponse = await response.json();

    console.log("Usuário encontrado:", resultado);

    return resultado;
}
