<template>
    <Disclosure as="nav" class="bg-slate-800" v-slot="{ open }">
        <Container class="mb-4">
            <div class="relative flex items-center justify-between h-16">
                <div class="absolute inset-y-0 left-0 flex items-center sm:hidden">
                    <DisclosureButton
                            class="inline-flex items-center justify-center p-2 rounded-md text-gray-400 hover:text-white hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-white">
                        <span class="sr-only">Open main menu</span>
                        <i v-if="!open" class='bx bx-menu text-3xl'></i>
                        <i v-else class='bx bx-x text-3xl'></i>
                    </DisclosureButton>
                </div>
                <div class="flex-1 flex items-center justify-center sm:items-stretch sm:justify-start">
                    <NuxtLink to="/"
                              class="flex-shrink-0 flex items-center font-extrabold text-xl text-transparent bg-clip-text bg-gradient-to-r from-green-500 to-primary-500">
                        SharpyProxy
                    </NuxtLink>
                    <div class="hidden sm:block sm:ml-6">
                        <div class="flex space-x-4">
                            <NuxtLink v-for="page in pages"
                                      :key="page.name"
                                      :to="page.href"
                                      active-class="bg-gray-900 text-white"
                                      class="text-gray-300 hover:bg-gray-700 hover:text-white px-3 py-2 rounded-md text-sm font-medium">
                                {{ page.name }}
                            </NuxtLink>
                        </div>
                    </div>
                </div>
            </div>
        </Container>
        <DisclosurePanel class="sm:hidden">
            <div class="px-2 pt-2 pb-3 space-y-1">
                <NuxtLink v-for="page in pages"
                          :key="page.name"
                          :to="page.href"
                          active-class="bg-slate-900 text-white"
                          class="text-slate-300 hover:bg-slate-700 hover:text-white block px-3 py-2 rounded-md text-base font-medium">
                    {{ page.name }}
                </NuxtLink>
            </div>
        </DisclosurePanel>
    </Disclosure>
</template>

<script setup lang="ts">
import {Disclosure, DisclosureButton, DisclosurePanel} from '@headlessui/vue';

const pages = [
    {name: "Dashboard", href: "/"},
    {name: "Clusters", href: "/clusters"},
    {name: "Routes", href: "/routes"},
    {name: "Certificates", href: "/certificates"},
    {name: "Audit log", href: "/log"}
]
</script>