<template>
    <Button type="button" @click="openCreationModal">
        Create cluster
    </Button>

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

            <div class="fixed inset-0 flex justify-center mt-8">
                <DialogPanel class="relative w-full h-fit p-4 max-w-xl rounded-lg bg-white">
                    <h2 class="text-xl font-bold text-slate-800 text-center">
                        Create new cluster
                    </h2>
                    <Separator/>
                    <form v-if="createModel" @submit.prevent="createCluster">
                        <div class="block text-sm font-semibold text-slate-700">
                            Cluster name
                        </div>
                        <div class="flex gap-x-4 items-center mt-2">
                            <div class="grow">
                                <input type="text"
                                       v-model="createModel.name"
                                       placeholder="Name (eg: cluster-1)"
                                       class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
                            </div>
                        </div>
                        <Toggle v-model="createModel.enabled" class="mt-4 text-sm font-semibold text-slate-700">
                            Enabled
                        </Toggle>
                        <div class="block text-sm font-semibold text-slate-700 mt-4">
                            Destinations
                        </div>
                        <div v-for="(destination, index) in createModel.destinations"
                             :key="destination"
                             class="flex items-center gap-x-2 mt-2">
                            <div class="grow-0">
                                <input type="text"
                                       v-model="destination.name"
                                       placeholder="Name (eg: dest-1)"
                                       class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
                            </div>
                            <div class="grow">
                                <input type="text"
                                       v-model="destination.address"
                                       placeholder="Address (eg: http://localhost:3000)"
                                       class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
                            </div>
                            <div v-if="index == 0" class="grow-0 flex">
                                <i @click="createModel.destinations.push(new CreateClusterDestinationModel())"
                                   class="bx bx-plus-circle text-2xl text-primary-500 hover:text-primary-600 cursor-pointer"
                                   title="Add destination">
                                </i>
                            </div>
                            <div v-else class="grow-0 flex">
                                <i @click="createModel.destinations.splice(createModel.destinations.indexOf(destination), 1)"
                                   class="bx bx-minus-circle text-2xl text-red-500 hover:text-red-600 cursor-pointer"
                                   title="Remove destination">
                                </i>
                            </div>
                        </div>
                        <div class="flex mt-4 gap-x-2">
                            <Button type="submit" class="grow">
                                Create
                            </Button>
                            <Button type="button" class="grow" @click="closeCreationModal"
                                    :style="ButtonStyle.RedOutline">
                                Cancel
                            </Button>
                        </div>
                    </form>
                </DialogPanel>
            </div>
        </Dialog>
    </TransitionRoot>
</template>

<script setup lang="ts">
import {Dialog, DialogOverlay, DialogPanel, TransitionChild, TransitionRoot} from "@headlessui/vue";
import {Ref} from "vue";
import CreateClusterModel from "~/models/cluster/CreateClusterModel";
import CreateClusterDestinationModel from "~/models/cluster/destination/CreateClusterDestinationModel";
import ButtonStyle from "~/types/ButtonStyle";

const emit = defineEmits(["cluster-created"]);

const createModalOpen: Ref<boolean> = ref(false);
const createModel: Ref<CreateClusterModel | undefined> = ref();

const httpClient = useClustersHttpClient();

async function createCluster() {
    await httpClient.create(createModel.value!);
    closeCreationModal();
    emit("cluster-created");
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