<script setup lang="ts">
import Navbar from '@/components/Navbar.vue'
</script>

<template>
  <div class="flex min-h-screen flex-col">
    <Navbar />
    <main class="mx-auto w-full max-w-5xl flex-1 px-4 py-8 sm:px-6">
      <RouterView />
    </main>
    <footer class="border-t border-line py-6 text-center font-mono text-[11px] uppercase tracking-widest text-ink-faint">
      Notebook — a small place for your notes
    </footer>
  </div>
</template>
