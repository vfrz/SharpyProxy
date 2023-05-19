<template>
  <div @click="openModal" class="text-primary-500 hover:text-primary-800 cursor-pointer">
    Edit
  </div>
  <Modal :opened="modalOpened">
    <ModalTitle>
      Edit cluster
    </ModalTitle>
    <Separator/>
    <Form v-if="updateModel" @submit="save">
      <div class="block text-sm font-semibold text-slate-700">
        Cluster name
      </div>
      <div class="flex gap-x-4 items-center mt-2">
        <div class="grow">
          <input type="text"
                 v-model="updateModel.name"
                 placeholder="Name (eg: cluster-1)"
                 class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
        </div>
      </div>
      <Toggle v-model="updateModel.enabled" class="mt-4 text-sm font-semibold text-slate-700">
        Enabled
      </Toggle>
      <div class="block text-sm font-semibold text-slate-700 mt-4">
        Destinations
      </div>
      <div v-for="(destination, index) in updateModel.destinations"
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
          <i @click="updateModel.destinations.push({name: '', address: ''})"
             class="bx bx-plus-circle text-2xl text-primary-500 hover:text-primary-600 cursor-pointer"
             title="Add destination">
          </i>
        </div>
        <div v-else class="grow-0 flex">
          <i @click="updateModel.destinations.splice(updateModel.destinations.indexOf(destination), 1)"
             class="bx bx-minus-circle text-2xl text-red-500 hover:text-red-600 cursor-pointer"
             title="Remove destination">
          </i>
        </div>
      </div>
      <div class="grid grid-cols-2 mt-4 gap-x-2">
        <Button type="submit">
          Save
        </Button>
        <Button type="button"
                @click="closeModal"
                :style="ButtonStyle.RedOutline">
          Cancel
        </Button>
      </div>
    </Form>
  </Modal>
</template>

<script setup lang="ts">
import {Ref} from "vue";
import ButtonStyle from "~/types/ButtonStyle";
import UpdateClusterModel from "~/models/cluster/UpdateClusterModel";
import UpdateClusterDestinationModel from "~/models/cluster/destination/UpdateClusterDestinationModel";

const props = defineProps({
    clusterId: {
        type: String,
        required: true
    }
})

const emit = defineEmits(["cluster-updated"]);
const httpClient = useClustersHttpClient();

const modalOpened: Ref<boolean> = ref(false);
const updateModel: Ref<UpdateClusterModel | undefined> = ref();

async function save() {
    await httpClient.update(updateModel.value!);
    closeModal();
    emit("cluster-updated");
}

async function openModal() {
    let cluster = await httpClient.get(props.clusterId!);
    let model = new UpdateClusterModel();
    model.id = cluster.id;
    model.name = cluster.name;
    model.enabled = cluster.enabled;
    model.destinations = cluster.destinations.map(d => new UpdateClusterDestinationModel(d.name, d.address));
    updateModel.value = model;
    modalOpened.value = true;
}

function closeModal() {
    modalOpened.value = false;
}
</script>