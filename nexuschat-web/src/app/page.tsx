import { redirect } from "next/navigation";
import BuscarAmigo from "@/app/components/amigos/buscarAmigo";

export default function Home() {
  return (
      <main className="flex min-h-screen items-center justify-center bg-[#15121f] p-6">
        <BuscarAmigo />
      </main>
  );
}