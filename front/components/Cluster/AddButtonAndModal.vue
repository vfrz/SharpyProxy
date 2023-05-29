<template>
  <Button type="button" @click="openModal">
    Add cluster
  </Button>
  <Modal :opened="modalOpened">
    <ModalTitle>
      New cluster
    </ModalTitle>
    <Separator/>
    <Form v-if="formModel" @submit="save" :validation-schema="validationSchema" v-slot="{errors}">
      <div class="block text-sm font-semibold text-slate-700">
        Cluster name
      </div>
      <div class="flex gap-x-4 items-center mt-2">
        <div class="grow">
          <Field type="text"
                 name="name"
                 v-model="formModel.name"
                 :validate-on-input="true"
                 :validate-on-blur="false"
                 placeholder="Name (eg: cluster-1)"
                 :class="errors.name && '!ring-red-500 !border-red-500'"
                 class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
          <ErrorMessage name="name" as="div"
                        class="text-sm font-semibold text-red-500 first-letter:uppercase mt-1"/>
        </div>
      </div>
      <Toggle v-model="formModel.enabled" class="mt-4 text-sm font-semibold text-slate-700">
        Enabled
      </Toggle>
      <div class="block text-sm font-semibold text-slate-700 mt-4">
        Destinations
      </div>
      <div v-for="(destination, index) in formModel.destinations"
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
          <i @click="formModel.destinations.push({address: '', name: ''})"
             class="bx bx-plus-circle text-2xl text-primary-500 hover:text-primary-600 cursor-pointer"
             title="Add destination">
          </i>
        </div>
        <div v-else class="grow-0 flex">
          <i @click="formModel.destinations.splice(formModel.destinations.indexOf(destination), 1)"
             class="bx bx-minus-circle text-2xl text-red-500 hover:text-red-600 cursor-pointer"
             title="Remove destination">
          </i>
        </div>
      </div>
      <div class="grid grid-cols-2 mt-4 gap-x-2">
        <Button type="submit">
          Save
        </Button>
        <Button type="button" @click="closeModal" :style="ButtonStyle.RedOutline">
          Cancel
        </Button>
      </div>
    </Form>
  </Modal>
</template>

<script setup lang="ts">
import {reactive, Ref, ref} from "vue";
import ButtonStyle from "~/types/ButtonStyle";
import CreateClusterModel from "~/models/cluster/CreateClusterModel";
import * as zod from "zod";

const emit = defineEmits(["cluster-created"]);

const httpClient = useClustersHttpClient();

const modalOpened: Ref<boolean> = ref(false);
const formModel: CreateClusterModel = reactive({
    name: "",
    enabled: true,
    destinations: []
});

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

async function save() {
    let response = await httpClient.create(formModel);
    if (response.success) {
        closeModal();
        emit("cluster-created");
    } else {
        //TODO: implement notifications
        alert("Failed to create cluster: " + response.message);
    }
}

function openModal() {
    formModel.name = "";
    formModel.enabled = true;
    formModel.destinations = [{
        name: '',
        address: ''
    }];

    modalOpened.value = true;
}

function closeModal() {
    modalOpened.value = false;
}
</script>