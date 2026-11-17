import { Bricolage_Grotesque, Figtree } from "next/font/google";

const titulo = Bricolage_Grotesque({
    subsets: ["latin"],
    variable: "--font-titulo",
});
const corpo = Figtree({
    subsets: ["latin"],
    variable: "--font-corpo",
});

export default function AuthLayout({
                                       children,
                                   }: {
    children: React.ReactNode;
}) {
    return (
        <div
            className={`${titulo.variable} ${corpo.variable} font-[family-name:var(--font-corpo)] flex min-h-screen w-full flex-col bg-[#15121f] p-6 text-[#ece9f5] sm:p-10`}
        >
            <header>
                <span className="font-[family-name:var(--font-titulo)] text-2xl font-bold tracking-tight">
                    Kova
                </span>
            </header>

            <main className="flex flex-1 items-center justify-center py-8">
                {children}
            </main>
        </div>
    );
}