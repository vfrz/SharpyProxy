<template>
  <div @click="openModal" class="text-primary-500 hover:text-primary-700 cursor-pointer">
    Edit
  </div>
  <Modal :opened="modalOpened">
    <ModalTitle>
      Edit route
    </ModalTitle>
    <Separator/>
    <Form v-if="updateModel" @submit="save" :validation-schema="validationSchema" v-slot="{errors}">
      <div class="block text-sm font-semibold text-slate-700">
        Route name
      </div>
      <div class="flex gap-x-4 items-center mt-1">
        <div class="grow">
          <Field type="text"
                 name="name"
                 v-model="updateModel.name"
                 :validate-on-input="true"
                 :validate-on-blur="false"
                 placeholder="Name (eg: route-1)"
                 :class="errors.name && '!ring-danger-500 !border-danger-500'"
                 class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
          <ErrorMessage name="name" as="div"
                        class="text-sm font-semibold text-danger-500 first-letter:uppercase mt-1"/>
        </div>
      </div>

      <Toggle v-model="updateModel.enabled" class="mt-4 text-sm font-semibold text-slate-700">
        Enabled
      </Toggle>

      <Listbox as="div" v-model="updateModel.cluster" class="mt-4">
        <ListboxLabel class="block text-sm font-semibold text-slate-700">
          Cluster
        </ListboxLabel>
        <div class="mt-1 relative">
          <ListboxButton
              class="relative w-full bg-white border border-slate-300 rounded-md shadow-sm pl-3 pr-10 py-2 text-left cursor-default focus:outline-none focus:ring-1 focus:ring-primary-500 focus:border-primary-500 sm:text-sm">
            <div class="flex items-center">
              <span
                  :class="[updateModel.cluster?.enabled ? 'bg-green-400' : 'bg-orange-400', 'flex-shrink-0 inline-block h-2 w-2 rounded-full']"/>
              <span class="ml-3 block truncate">{{ updateModel.cluster?.name }}</span>
            </div>
            <span class="absolute inset-y-0 right-0 flex items-center pr-2 pointer-events-none">
              <i class="bx bxs-sort-alt h-5 w-5 text-xl text-slate-400"></i>
            </span>
          </ListboxButton>

          <transition leave-active-class="transition ease-in duration-100" leave-from-class="opacity-100"
                      leave-to-class="opacity-0">
            <ListboxOptions
                class="absolute z-10 mt-1 w-full bg-white shadow-lg max-h-60 rounded-md py-1 text-base ring-1 ring-black ring-opacity-5 overflow-auto focus:outline-none sm:text-sm">
              <ListboxOption as="template" v-for="cluster in clusters" :key="cluster.id" :value="cluster"
                             v-slot="{ active, selected }">
                <li :class="[active ? 'text-white bg-primary-600' : 'text-gray-900', 'cursor-pointer select-none relative py-2 pl-3 pr-9']">
                  <div class="flex items-center">
                    <span aria-hidden="true"
                          :class="[cluster.enabled ? 'bg-green-400' : 'bg-orange-400', 'flex-shrink-0 inline-block h-2 w-2 rounded-full']"/>
                    <span :class="[selected ? 'font-semibold' : 'font-normal', 'ml-3 block truncate']">
                      {{ cluster.name }}
                    </span>
                  </div>
                  <span v-if="selected"
                        :class="[active ? 'text-white' : 'text-primary-600', 'absolute inset-y-0 right-0 flex items-center pr-4']">
                    <i class="bx bx-check text-xl"></i>
                  </span>
                </li>
              </ListboxOption>
            </ListboxOptions>
          </transition>
        </div>
      </Listbox>

      <div class="block text-sm font-semibold text-slate-700 mt-4">
        Matching hosts <span class="italic text-slate-500 font-normal">(comma separated)</span>
      </div>
      <div class="flex gap-x-4 items-center mt-1">
        <div class="grow">
          <Field type="text"
                 name="match-hosts"
                 v-model="updateModel.matchHosts"
                 placeholder="Hosts (eg: google.com,amazon.com)"
                 class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
        </div>
      </div>

      <div class="block text-sm font-semibold text-slate-700 mt-4">
        Matching path
      </div>
      <div class="flex gap-x-4 items-center mt-1">
        <div class="grow">
          <Field type="text"
                 name="match-path"
                 v-model="updateModel.matchPath"
                 placeholder="Path (eg: /something/{**remainder})"
                 class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
        </div>
      </div>

      <div class="grid grid-cols-2 mt-4 gap-x-2">
        <Button type="submit">
          Save
        </Button>
        <Button type="button" @click="closeModal" :style="ButtonStyle.DangerOutline">
          Cancel
        </Button>
      </div>
    </Form>
  </Modal>
</template>

<script setup lang="ts">
import {Ref, ref} from "vue";
import ButtonStyle from "~/types/ButtonStyle";
import * as zod from "zod";
import useRoutesHttpClient from "~/composables/useRoutesHttpClient";
import ClusterModel from "~/models/cluster/ClusterModel";
import {Listbox, ListboxButton, ListboxLabel, ListboxOption, ListboxOptions} from "@headlessui/vue";
import UpdateRouteViewModel from "~/models/route/UpdateRouteViewModel";
import UpdateRouteModel from "~/models/route/UpdateRouteModel";

const props = defineProps({
    routeId: {
        type: String,
        required: true
    }
})

const emit = defineEmits(["route-updated"]);

const httpClient = useRoutesHttpClient();
const clustersHttpClient = useClustersHttpClient();
const stringHelper = useStringHelper();

const modalOpened: Ref<boolean> = ref(false);
const clusters: Ref<ClusterModel[]> = ref([]);

const updateModel: Ref<UpdateRouteViewModel | undefined> = ref();

const validationSchema = toTypedSchema(
    zod.object({
        name: zod.string().trim().nonempty("Name can't be empty.")
    })
);

async function save() {
    //TODO Cluster + match path/hosts validation
    //hosts regex: ^(?:[a-z0-9](?:[a-z0-9-]{0,61}[a-z0-9])?\.)+[a-z0-9][a-z0-9-]{0,61}[a-z0-9](,(?:[a-z0-9](?:[a-z0-9-]{0,61}[a-z0-9])?\.)+[a-z0-9][a-z0-9-]{0,61}[a-z0-9])*$
    let updateRouteModel: UpdateRouteModel = {
        id: props.routeId!,
        name: updateModel.value!.name,
        enabled: updateModel.value!.enabled,
        clusterId: updateModel.value!.cluster?.id!,
        matchPath: stringHelper.isNullOrWhitespace(updateModel.value!.matchPath) ? undefined : updateModel.value!.matchPath,
        matchHosts: stringHelper.isNullOrWhitespace(updateModel.value!.matchHosts) ? undefined : updateModel.value!.matchHosts.split(",")
    };
    
    let response = await httpClient.update(updateRouteModel);
    if (response.success) {
        closeModal();
        emit("route-updated");
    } else {
        //TODO: implement notifications
        alert("Failed to update route: " + response.message);
    }
}

async function openModal() {
    await loadClusters();
    let route = await httpClient.get(props.routeId!);
    updateModel.value = {
        name: route.name,
        cluster: clusters.value.find(c => c.id === route.clusterId),
        enabled: route.enabled,
        matchPath: route.matchPath ?? "",
        matchHosts: route.matchHosts?.join(",") ?? ""
    };
    modalOpened.value = true;
}

function closeModal() {
    modalOpened.value = false;
}

async function loadClusters() {
    clusters.value = await clustersHttpClient.list();
}
</script>