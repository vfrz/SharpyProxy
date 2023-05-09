<template>
    <Title>SharpyProxy - Clusters</Title>
    <Container>
        <div class="sm:flex sm:items-center">
            <div class="sm:flex-auto">
                <h1 class="text-xl font-semibold text-slate-900">
                    Clusters
                    <template v-if="clusters">
                        ({{ clusters.length }})
                    </template>
                </h1>
                <p class="mt-2 text-sm text-gray-700">
                    A list of all the clusters on your SharpyProxy instance.
                </p>
            </div>
            <div class="mt-4 sm:mt-0 sm:ml-16 sm:flex-none">
                <button type="button"
                        @click="openCreationModal"
                        class="inline-flex items-center justify-center rounded-md border border-transparent bg-primary-500 px-4 py-2 text-sm font-medium text-white shadow-sm hover:bg-primary-600 sm:w-auto">
                    Add cluster
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
                                    Destinations
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
                            <tr v-for="cluster in clusters" :key="cluster.id">
                                <td class="whitespace-nowrap py-4 pl-4 pr-3 text-sm sm:pl-6 font-medium">
                                    {{ cluster.id }}
                                </td>
                                <td class="whitespace-nowrap px-3 py-4 text-sm text-gray-500">
                                    {{ cluster.destinations }}
                                </td>
                                <td class="whitespace-nowrap px-3 py-4 text-sm text-gray-500">
                                    <EnabledTag :enabled="cluster.enabled"/>
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

    <TransitionRoot
            :show="createModalOpen"
            as="template"
            enter="duration-200 ease-out"
            enter-from="opacity-0"
            enter-to="opacity-100"
            leave="duration-200 ease-in"
            leave-from="opacity-100"
            leave-to="opacity-0">
        <Dialog :static="true" class="relative z-40">
            <TransitionChild as="template" enter="ease-out duration-300" enter-from="opacity-0" enter-to="opacity-100"
                             leave="ease-in duration-200" leave-from="opacity-100" leave-to="opacity-0">
                <DialogOverlay class="fixed inset-0 bg-slate-800 bg-opacity-80 transition-opacity"/>
            </TransitionChild>

            <div class="fixed inset-0 flex justify-center p-4">
                <DialogPanel class="relative w-full h-fit p-4 max-w-xl rounded-lg bg-white">
                    <div class="absolute top-0 right-0 pt-5 pr-4">
                        <button type="button" class="text-slate-800 hover:text-slate-600 !outline-none"
                                @click="closeCreationModal">
                            <span class="sr-only">Close</span>
                            <i class="bx bx-x text-2xl"></i>
                        </button>
                    </div>

                    <h2 class="text-xl font-bold flex items-center text-slate-800">
                        Create new cluster
                    </h2>

                    <form @submit.prevent="createCluster">

                        <div class="block text-sm font-medium text-gray-700 mt-2">
                            Cluster id
                        </div>
                        <input type="text"
                               v-model="createModel.id"
                               placeholder="Identifier"
                               class="shadow-sm mt-2 focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
                        <div class="block text-sm font-medium text-gray-700 mt-2">
                            Destinations
                        </div>
                        <div v-for="(destination, index) in createModel.destinations"
                             class="flex items-center gap-x-2 mt-2">
                            <div class="grow-0">
                                <input type="text"
                                       v-model="destination.id"
                                       placeholder="Identifier"
                                       class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
                            </div>
                            <div class="grow">
                                <input type="text"
                                       v-model="destination.address"
                                       placeholder="Address"
                                       class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
                            </div>
                            <div v-if="index == 0" class="grow-0 align-middle justify-center">
                                <i @click="createModel.destinations.push(new CreateClusterDestinationModel())"
                                   class="bx bx-plus-circle text-2xl text-primary-500 cursor-pointer"></i>
                            </div>
                            <div v-else class="grow-0 align-middle justify-center">
                                <i @click="createModel.destinations.splice(createModel.destinations.indexOf(destination), 1)"
                                   class="bx bx-minus-circle text-2xl text-red-500 cursor-pointer"></i>
                            </div>
                        </div>
                        <div class="mt-4">
                            <button type="submit"
                                    class="inline-flex justify-center py-2 px-4 border border-transparent shadow-sm text-sm font-medium rounded-md text-white bg-primary-500 hover:bg-primary-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-500">
                                Create
                            </button>
                        </div>
                    </form>
                </DialogPanel>
            </div>
        </Dialog>
    </TransitionRoot>
</template>

<script setup lang="ts">
import ClusterModel from "~/models/cluster/ClusterModel";
import {Ref} from "vue";
import EnabledTag from "~/components/EnabledTag.vue";
import {
    Dialog,
    DialogPanel,
    DialogOverlay,
    TransitionChild,
    TransitionRoot,
} from "@headlessui/vue";
import CreateClusterModel from "~/models/cluster/CreateClusterModel";
import CreateClusterDestinationModel from "~/models/cluster/destination/CreateClusterDestinationModel";

const httpClient = useClustersHttpClient();

const clusters: Ref<ClusterModel[]> = ref();

const createModalOpen: Ref<boolean> = ref(false);
const createModel: Ref<CreateClusterModel> = ref();

onNuxtReady(async () => {
    await reloadClusters();
});

async function reloadClusters() {
    clusters.value = await httpClient.list();
}

async function createCluster() {
    await httpClient.create(createModel.value);
    closeCreationModal();
    await reloadClusters();
}

function openCreationModal() {
    let model = new CreateClusterModel();
    model.destinations.push(new CreateClusterDestinationModel());
    createModel.value = model;
    createModalOpen.value = true;
}

function closeCreationModal() {
    createModalOpen.value = false;
}

</script>