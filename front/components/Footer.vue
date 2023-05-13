<template>
    <footer class="bg-white">
        <Container class="py-6 sm:px-6 md:flex md:items-center md:justify-between lg:px-8">
            <div class="flex justify-center space-x-6 md:order-2">
                <NuxtLink to="https://github.com/vfrz/SharpyProxy" target="_blank"
                   class="flex text-slate-400 hover:text-slate-500">
                    <i class="bx bxl-github text-2xl"></i>
                </NuxtLink>
            </div>
            <div class="mt-8 md:mt-0 md:order-1">
                <p class="text-center text-base text-slate-400">
                    SharpyProxy - v{{ config.public.version }}
                </p>
            </div>
        </Container>
    </footer>
</template>

<script setup lang="ts">
const config = useRuntimeConfig();
</script>