"use client";

import { useState, FormEvent } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { Eye, EyeOff } from "lucide-react";
import { loginUsuario } from "@/services/authService";

export default function LoginPage() {
    const router = useRouter();
    const [nomeDeUsuario, setNomeDeUsuario] = useState("");
    const [senha, setSenha] = useState("");
    const [mostrarSenha, setMostrarSenha] = useState(false);
    const [carregando, setCarregando] = useState(false);
    const [erro, setErro] = useState<string | null>(null);

    async function handleSubmit(e: FormEvent) {
        e.preventDefault();
        setErro(null);
        setCarregando(true);

        try {
            await loginUsuario({ nomeDeUsuario, senha });
            router.push("/"); // troque pela rota principal do seu app
        } catch (err: unknown) {
            setErro(
                err instanceof Error && err.message
                    ? err.message
                    : "Nome de usuário ou senha inválidos."
            );
        } finally {
            setCarregando(false);
        }
    }

    const labelClass = (temErro: boolean) =>
        `mb-2 block text-sm font-semibold ${
            temErro ? "text-[#f0838f]" : "text-[#b4aecb]"
        }`;

    const inputClass =
        "w-full rounded-md border border-[#2e2940] bg-[#110e19] px-3.5 py-2.5 text-[15px] text-[#ece9f5] outline-none focus:outline-2 focus:outline-offset-0 focus:outline-[#8b6cf0]";

    return (
        <div className="grid w-full max-w-[880px] overflow-hidden rounded-md border border-[#2e2940] bg-[#1d1929] md:grid-cols-[1.05fr_1fr]">
            {/* Prévia do chat */}
            <aside className="hidden flex-col border-r border-[#2e2940] bg-[#17141f] md:flex">
                <div className="border-b border-[#2e2940] px-6 py-4">
                    <p className="font-[family-name:var(--font-titulo)] font-bold">
                        <span className="text-[#8b6cf0]">#</span> geral
                    </p>
                    <p className="mt-0.5 text-xs text-[#9d97b5]">
                        Onde a galera combina tudo
                    </p>
                </div>

                <div className="flex flex-1 flex-col justify-end gap-5 px-6 py-6 text-sm">
                    <div className="flex gap-3">
                        <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-[#e0856b] font-bold text-[#1d1929]">
                            N
                        </div>
                        <div>
                            <p>
                                <span className="font-semibold text-[#e0856b]">nova</span>
                                <span className="ml-2 text-[11px] text-[#7d7794]">
                                    Hoje às 21:04
                                </span>
                            </p>
                            <p className="mt-0.5 leading-snug text-[#d8d4e8]">
                                alguém pra rankeada hoje à noite?
                            </p>
                        </div>
                    </div>

                    <div className="flex gap-3">
                        <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-[#6bb5a6] font-bold text-[#1d1929]">
                            L
                        </div>
                        <div>
                            <p>
                                <span className="font-semibold text-[#6bb5a6]">
                                    lua
                                </span>
                                <span className="ml-2 text-[11px] text-[#7d7794]">
                                    Hoje às 21:05
                                </span>
                            </p>
                            <p className="mt-0.5 leading-snug text-[#d8d4e8]">
                                eu entro depois das 22h pessoal
                            </p>
                        </div>
                    </div>

                    <div className="flex gap-3">
                        <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-[#c99bd9] font-bold text-[#1d1929]">
                            L
                        </div>
                        <div>
                            <p>
                                <span className="font-semibold text-[#c99bd9]">luna</span>
                                <span className="ml-2 text-[11px] text-[#7d7794]">
                                    Hoje às 21:07
                                </span>
                            </p>
                            <p className="mt-1 border-l-2 border-[#3a3350] pl-2 text-xs text-[#9d97b5]">
                                lua: eu entro depois das 22h pessoal
                            </p>
                            <p className="mt-1 leading-snug text-[#d8d4e8]">
                                fechou então, pessoal. até daqui a pouco
                            </p>
                        </div>
                    </div>

                    <p className="text-xs text-[#7d7794]">panic está digitando…</p>
                </div>

                <div className="mx-6 mb-6 rounded-md bg-[#211d2e] px-3.5 py-2.5 text-sm text-[#7d7794]">
                    Conversar em #geral
                </div>
            </aside>

            {/* Formulário */}
            <section className="p-8 sm:p-10">
                <div className="mb-7">
                    <h1 className="font-[family-name:var(--font-titulo)] text-3xl font-bold tracking-tight">
                       Tá esperando o que? A galera tá te esperando.
                    </h1>
                    <p className="mt-2 text-[15px] text-[#9d97b5]">
                        Faça o login, e vem conversar.
                    </p>
                </div>

                <form onSubmit={handleSubmit} noValidate>
                    <div className="mb-5">
                        <label htmlFor="login-usuario" className={labelClass(!!erro)}>
                            Nome de usuário
                            {erro && (
                                <span className="font-normal italic"> — {erro}</span>
                            )}
                        </label>
                        <input
                            id="login-usuario"
                            type="text"
                            required
                            autoComplete="username"
                            value={nomeDeUsuario}
                            onChange={(e) => setNomeDeUsuario(e.target.value)}
                            className={inputClass}
                        />
                    </div>

                    <div>
                        <label htmlFor="login-senha" className={labelClass(!!erro)}>
                            Senha
                        </label>
                        <div className="relative">
                            <input
                                id="login-senha"
                                type={mostrarSenha ? "text" : "password"}
                                required
                                autoComplete="current-password"
                                value={senha}
                                onChange={(e) => setSenha(e.target.value)}
                                className={`${inputClass} pr-10`}
                            />
                            <button
                                type="button"
                                onClick={() => setMostrarSenha(!mostrarSenha)}
                                aria-label={mostrarSenha ? "Ocultar senha" : "Mostrar senha"}
                                className="absolute right-3 top-1/2 -translate-y-1/2 cursor-pointer text-[#9d97b5] hover:text-white"
                            >
                                {mostrarSenha ? (
                                    <EyeOff className="h-4 w-4" />
                                ) : (
                                    <Eye className="h-4 w-4" />
                                )}
                            </button>
                        </div>
                        <span className="mt-2 inline-block cursor-pointer text-sm text-[#a690f5] hover:underline">
                            Esqueceu sua senha?
                        </span>
                    </div>

                    <button
                        type="submit"
                        disabled={carregando}
                        className="mt-6 w-full cursor-pointer rounded-md bg-[#8b6cf0] py-2.5 text-[15px] font-semibold text-white hover:bg-[#7a59e6] disabled:opacity-50"
                    >
                        {carregando ? "Entrando..." : "Entrar"}
                    </button>
                </form>

                <p className="mt-5 text-sm text-[#9d97b5]">
                    Ainda não tem conta?{" "}
                    <Link href="/auth/register" className="text-[#a690f5] hover:underline">
                        Registre-se
                    </Link>
                </p>
            </section>
        </div>
    );
}