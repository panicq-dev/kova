"use client";

import { useState, FormEvent } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { Eye, EyeOff } from "lucide-react";
import { registrarUsuario } from "@/services/authService";

export default function RegisterPage() {
    const router = useRouter();
    const [email, setEmail] = useState("");
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
            await registrarUsuario({ nomeDeUsuario, email, senha });
            router.push("/auth/login");
        } catch (err: unknown) {
            setErro(
                err instanceof Error && err.message
                    ? err.message
                    : "Não foi possível criar sua conta. Tente novamente."
            );
        } finally {
            setCarregando(false);
        }
    }

    const labelClass = "mb-2 block text-sm font-semibold text-[#b4aecb]";
    const inputClass =
        "w-full rounded-md border border-[#2e2940] bg-[#110e19] px-3.5 py-2.5 text-[15px] text-[#ece9f5] outline-none focus:outline-2 focus:outline-offset-0 focus:outline-[#8b6cf0]";

    return (
        <div className="w-full max-w-[440px] rounded-md border border-[#2e2940] bg-[#1d1929] p-8 sm:p-10">
            <div className="mb-7">
                <h1 className="font-[family-name:var(--font-titulo)] text-3xl font-bold tracking-tight">
                    Criar conta
                </h1>
                <p className="mt-2 text-[15px] text-[#9d97b5]">
                    Escolha como você vai aparecer pra galera.
                </p>
            </div>

            {erro && (
                <div
                    role="alert"
                    className="mb-5 rounded-md border border-[#f0838f]/40 bg-[#f0838f]/10 px-3.5 py-2.5 text-sm text-[#f8c6cb]"
                >
                    {erro}
                </div>
            )}

            <form onSubmit={handleSubmit} className="space-y-5">
                <div>
                    <label htmlFor="reg-email" className={labelClass}>
                        E-mail
                    </label>
                    <input
                        id="reg-email"
                        type="email"
                        required
                        autoComplete="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        className={inputClass}
                    />
                </div>

                <div>
                    <label htmlFor="reg-usuario" className={labelClass}>
                        Nome de usuário
                    </label>
                    <input
                        id="reg-usuario"
                        type="text"
                        required
                        autoComplete="username"
                        value={nomeDeUsuario}
                        onChange={(e) => setNomeDeUsuario(e.target.value)}
                        className={inputClass}
                    />
                </div>

                <div>
                    <label htmlFor="reg-senha" className={labelClass}>
                        Senha
                    </label>
                    <div className="relative">
                        <input
                            id="reg-senha"
                            type={mostrarSenha ? "text" : "password"}
                            required
                            autoComplete="new-password"
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
                </div>

                <button
                    type="submit"
                    disabled={carregando}
                    className="w-full cursor-pointer rounded-md bg-[#8b6cf0] py-2.5 text-[15px] font-semibold text-white hover:bg-[#7a59e6] disabled:opacity-50"
                >
                    {carregando ? "Criando conta..." : "Continuar"}
                </button>
            </form>

            <p className="mt-4 text-xs text-[#7d7794]">
                Ao se registrar, você concorda com os Termos de Serviço e a Política
                de Privacidade do Kova.
            </p>

            <Link
                href="/auth/login"
                className="mt-4 inline-block text-sm text-[#a690f5] hover:underline"
            >
                Já tem uma conta?
            </Link>
        </div>
    );
}