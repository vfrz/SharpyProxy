<template>
  <Button type="button" @click="openCreationModal">
    Add cluster
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
            New cluster
          </h2>
          <Separator/>
          <Form v-if="createModel" @submit="createCluster" :validation-schema="validationSchema" v-slot="{errors}">
            <div class="block text-sm font-semibold text-slate-700">
              Cluster name
            </div>
            <div class="flex gap-x-4 items-center mt-2">
              <div class="grow">
                <Field type="text"
                       name="name"
                       v-model="createModel.name"
                       :validate-on-input="true"
                       :validate-on-blur="false"
                       placeholder="Name (eg: cluster-1)"
                       :class="errors.name && '!ring-red-500 !border-red-500'"
                       class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
                <ErrorMessage name="name" as="div"
                              class="text-sm font-semibold text-red-500 first-letter:uppercase mt-1"/>
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
                <Field type="text"
                       :name="`destinations[${index}].name`"
                       :validate-on-input="true"
                       :validate-on-blur="false"
                       v-model="destination.name"
                       placeholder="Name (eg: dest-1)"
                       :class="errors[`destinations[${index}].name`] && '!ring-red-500 !border-red-500'"
                       class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
              </div>
              <div class="grow">
                <Field type="text"
                       :name="`destinations[${index}].address`"
                       :validate-on-input="true"
                       :validate-on-blur="false"
                       v-model="destination.address"
                       placeholder="Address (eg: http://localhost:3000)"
                       :class="errors[`destinations[${index}].address`] && '!ring-red-500 !border-red-500'"
                       class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
              </div>
              <div v-if="index == 0" class="grow-0 flex">
                <i @click="createModel.destinations.push({address: '', name: ''})"
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
                Save
              </Button>
              <Button type="button" class="grow" @click="closeCreationModal" :style="ButtonStyle.RedOutline">
                Cancel
              </Button>
            </div>
          </Form>
        </DialogPanel>
      </div>
    </Dialog>
  </TransitionRoot>
</template>

<script setup lang="ts">
import {Dialog, DialogOverlay, DialogPanel, TransitionChild, TransitionRoot} from "@headlessui/vue";
import {reactive, Ref, ref} from "vue";
import ButtonStyle from "~/types/ButtonStyle";
import CreateClusterModel from "~/models/cluster/CreateClusterModel";
import * as zod from "zod";

const emit = defineEmits(["cluster-created"]);

const createModalOpen: Ref<boolean> = ref(false);
const createModel: CreateClusterModel = reactive({
    name: "",
    enabled: true,
    destinations: []
});

const httpClient = useClustersHttpClient();

const destinationSchema = zod.object({
    name: zod.string().trim().nonempty("Name can't be empty."),
    address: zod.string().trim().nonempty("Address can't be empty.")
});

const validationSchema = toTypedSchema(
    zod.object({
        name: zod.string().trim().nonempty("Name can't be empty."),
        destinations: zod.array(destinationSchema).nonempty()
    })
);

async function createCluster() {
    let response = await httpClient.create(createModel);
    if (response.success) {
        closeCreationModal();
        emit("cluster-created");
    } else {
        //TODO: implement notifications
        alert("Failed to create cluster: " + response.message);
    }
}

function openCreationModal() {
    createModel.name = "";
    createModel.enabled = true;
    createModel.destinations = [{
        name: '',
        address: ''
    }];

    createModalOpen.value = true;
}

function closeCreationModal() {
    createModalOpen.value = false;
}
</script>