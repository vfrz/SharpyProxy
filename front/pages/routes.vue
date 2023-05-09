<template>
    <Title>SharpyProxy - Routes</Title>
    <Container>
        <div class="sm:flex sm:items-center">
            <div class="sm:flex-auto">
                <h1 class="text-xl font-semibold text-slate-900">
                    Routes
                    <template v-if="routes">
                        ({{ routes.length }})
                    </template>
                </h1>
                <p class="mt-2 text-sm text-gray-700">
                    A list of all the routes on your SharpyProxy instance.
                </p>
            </div>
            <div class="mt-4 sm:mt-0 sm:ml-16 sm:flex-none">
                <button type="button"
                        class="inline-flex items-center justify-center rounded-md border border-transparent bg-primary-500 px-4 py-2 text-sm font-medium text-white shadow-sm hover:bg-primary-600 sm:w-auto">
                    Add route
                </button>
            </div>
        </div>
        <div class="flex flex-col mt-4">
            <div class="-my-2 -mx-4 overflow-x-auto sm:-mx-6 lg:-mx-8">
                <div class="inline-block min-w-full py-2 align-middle md:px-6 lg:px-8">
                    <div class="overflow-hidden shadow ring-1 ring-black ring-opacity-5 md:rounded-lg">
                        <table class="min-w-full divide-y divide-slate-300">
                            <thead class="bg-slate-50">
                            <tr>
                                <th scope="col"
                                    class="py-3.5 px-4 pr-3 text-left text-sm font-semibold text-gray-900 sm:pl-6">
                                    Id
                                </th>
                                <th scope="col" class="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">
                                    Cluster
                                </th>
                                <th scope="col" class="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">
                                    Match hosts
                                </th>
                                <th scope="col" class="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">
                                    Match path
                                </th>
                                <th scope="col" class="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">
                                    Status
                                </th>
                                <th scope="col" class="relative py-3.5 pl-3 pr-4 sm:pr-6">
                                    <span class="sr-only">Edit</span>
                                </th>
                            </tr>
                            </thead>
                            <tbody class="divide-y divide-gray-200 bg-white">
                            <tr v-for="route in routes" :key="route.id">
                                <td class="whitespace-nowrap py-4 pl-4 pr-3 text-sm sm:pl-6 font-medium">
                                    {{ route.id }}
                                </td>
                                <td class="whitespace-nowrap px-3 py-4 text-sm text-gray-500">
                                    {{ route.clusterId }}
                                </td>
                                <td class="whitespace-nowrap px-3 py-4 text-sm text-gray-500">
                                    {{ route.matchHosts }}
                                </td>
                                <td class="whitespace-nowrap px-3 py-4 text-sm text-gray-500">
                                    {{ route.matchPath }}
                                </td>
                                <td class="whitespace-nowrap px-3 py-4 text-sm text-gray-500">
                                    <EnabledTag :enabled="route.enabled"/>
                                </td>
                                <td class="relative whitespace-nowrap py-4 pl-3 pr-4 text-right text-sm font-semibold sm:pr-6">
                                    <NuxtLink to="#" class="text-primary-500 hover:text-primary-800">
                                        Edit
                                    </NuxtLink>
                                </td>
                            </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </Container>
</template>

<script setup lang="ts">
import EnabledTag from "~/components/EnabledTag.vue";
import {Ref} from "vue";
import RouteModel from "~/models/route/RouteModel";

const httpClient = useRoutesHttpClient();

const routes: Ref<RouteModel[]> = ref();

onNuxtReady(async () => {
    await reloadRoutes();
});

async function reloadRoutes() {
    routes.value = await httpClient.list();
}

</script>