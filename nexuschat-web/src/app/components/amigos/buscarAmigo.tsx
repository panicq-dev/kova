
'use client';

import { FormEvent, useState } from "react";
import { buscarUsuario } from "@/services/usuarioService";
import { UsuarioResponse } from "@/types/usuario";

export default function BuscarAmigo() {
    const [nome, setNome] = useState("");
    const [usuario, setUsuario] = useState<UsuarioResponse | null>(null);
    const [carregando, setCarregando] = useState(false);
    const [erro, setErro] = useState("");
    const [pesquisou, setPesquisou] = useState(false);

    async function handleBuscarUsuario(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        if (!nome.trim()) return;

        setCarregando(true);
        setErro("");
        setUsuario(null);
        setPesquisou(true);

        try {
            const resultado = await buscarUsuario(nome.trim());
            setUsuario(resultado);
        } catch (error) {
            const mensagem = error instanceof Error ? error.message : "";

            if (mensagem.includes("404")) {
                setErro("Nenhum usuário encontrado com esse nome.");
            } else {
                setErro("Não foi possível realizar a pesquisa. Tente novamente.");
            }
        } finally {
            setCarregando(false);
        }
    }

    return (
        <section className="w-full max-w-xl rounded-2xl border border-white/[0.08] bg-[#1d1929] p-5 sm:p-7">

            <div className="mb-6">
                <h2 className="text-lg font-semibold text-[#ece9f5]">
                    Adicionar amigos
                </h2>
                <p className="mt-1 text-sm text-[#9891a9]">
                    Pesquise pelo nome de usuário para encontrar pessoas.
                </p>
            </div>

            <form onSubmit={handleBuscarUsuario}>
                <label
                    htmlFor="nomeUsuario"
                    className="mb-2 block text-sm font-medium text-[#d8d2e5]"
                >
                    Nome de usuário
                </label>

                <div className="flex flex-col gap-3 sm:flex-row">
                    <input
                        id="nomeUsuario"
                        type="text"
                        value={nome}
                        onChange={(e) => setNome(e.target.value)}
                        placeholder="Digite o nome de usuário..."
                        autoComplete="off"
                        className="h-11 min-w-0 flex-1 rounded-lg border border-white/[0.08] bg-[#15121f] px-4 text-sm text-[#ece9f5] outline-none transition placeholder:text-[#706980] focus:border-purple-500 focus:ring-2 focus:ring-purple-500/15"
                    />

                    <button
                        type="submit"
                        disabled={carregando || !nome.trim()}
                        className="h-11 rounded-lg bg-purple-600 px-6 text-sm font-semibold text-white transition hover:bg-purple-500 disabled:cursor-not-allowed disabled:opacity-50"
                    >
                        {carregando ? "Buscando..." : "Pesquisar"}
                    </button>
                </div>
            </form>

            <div className="mt-6 border-t border-white/[0.07] pt-5">
                <h3 className="mb-4 text-sm font-semibold text-[#d8d2e5]">
                    Resultados
                </h3>

                {!pesquisou && !carregando && (
                    <p className="py-6 text-center text-sm text-[#817990]">
                        Pesquise um usuário para visualizar os resultados.
                    </p>
                )}

                {carregando && (
                    <div className="flex items-center justify-center gap-3 py-6">
                        <div className="h-5 w-5 animate-spin rounded-full border-2 border-purple-500/20 border-t-purple-400" />
                        <span className="text-sm text-[#9891a9]">
                            Buscando usuário...
                        </span>
                    </div>
                )}

                {!carregando && erro && (
                    <p className="rounded-lg bg-red-400/[0.06] px-4 py-4 text-center text-sm text-red-300">
                        {erro}
                    </p>
                )}

                {!carregando && usuario && (
                    <div className="flex flex-col gap-4 rounded-xl border border-white/[0.07] bg-[#15121f] p-4 sm:flex-row sm:items-center sm:justify-between">
                        <div className="flex min-w-0 items-center gap-3">
                            <div className="flex h-11 w-11 shrink-0 items-center justify-center rounded-full bg-gradient-to-br from-purple-500 to-violet-800 font-semibold text-white">
                                {usuario.nomeDeUsuario.charAt(0).toUpperCase()}
                            </div>

                            <div className="min-w-0">
                                <p className="truncate text-sm font-semibold text-[#ece9f5]">
                                    {usuario.nomeDeUsuario}
                                </p>
                                <p className="text-xs text-[#817990]">
                                    Usuário do Kova
                                </p>
                            </div>
                        </div>

                        <button
                            type="button"
                            disabled
                            className="h-9 shrink-0 cursor-not-allowed rounded-lg border border-purple-400/20 bg-purple-500/10 px-4 text-sm font-medium text-purple-300/60"
                        >
                            + Adicionar
                        </button>
                    </div>
                )}
            </div>
        </section>
    );
}
