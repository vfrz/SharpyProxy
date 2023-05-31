<template>
  <div @click="openModal" class="text-rose-500 hover:text-rose-700 cursor-pointer">
    Delete
  </div>
  <TransitionRoot as="template" :show="modalOpened">
    <Dialog as="div" class="fixed z-10 inset-0 overflow-y-auto">
      <div class="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
        <TransitionChild as="template" enter="ease-out duration-300" enter-from="opacity-0" enter-to="opacity-100"
                         leave="ease-in duration-200" leave-from="opacity-100" leave-to="opacity-0">
          <DialogOverlay class="fixed inset-0 bg-slate-500 bg-opacity-75 transition-opacity"/>
        </TransitionChild>

        <!-- This element is to trick the browser into centering the modal contents. -->
        <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>
        <TransitionChild as="template" enter="ease-out duration-300"
                         enter-from="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                         enter-to="opacity-100 translate-y-0 sm:scale-100" leave="ease-in duration-200"
                         leave-from="opacity-100 translate-y-0 sm:scale-100"
                         leave-to="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95">
          <div
              class="relative inline-block align-bottom bg-white rounded-lg px-4 pt-5 pb-4 text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full sm:pt-6">
            <div class="sm:flex sm:items-start">
              <div
                  class="mx-auto flex-shrink-0 flex items-center justify-center h-12 w-12 rounded-full bg-danger-100 sm:mx-0 sm:h-10 sm:w-10">
                <i class="bx bx-error text-danger-600 text-2xl justify-self-center"></i>
              </div>
              <div class="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left">
                <DialogTitle as="h3" class="text-lg leading-6 font-medium text-slate-900">
                  Delete cluster
                </DialogTitle>
                <div class="mt-2">
                  <p class="text-sm text-slate-500">
                    Are you sure you want to delete the cluster "<span
                      class="font-semibold text-slate-600">{{ props.clusterName }}</span>"?
                    <br/>
                    The cluster and all linked routes will be permanently removed.
                    <br/>
                    This action cannot be undone.
                  </p>
                </div>
              </div>
            </div>
            <div class="mt-5 sm:mt-4 sm:flex sm:flex-row-reverse">
              <Button :style="ButtonStyle.Danger"
                      class="w-full sm:ml-2 sm:w-auto sm:text-sm"
                      @click="delete_">
                Delete
              </Button>
              <Button :style="ButtonStyle.Neutral"
                      class="mt-2 w-full sm:mt-0 sm:w-auto sm:text-sm"
                      @click="closeModal">
                Cancel
              </Button>
            </div>
          </div>
        </TransitionChild>
      </div>
    </Dialog>
  </TransitionRoot>
</template>

<script setup lang="ts">
import {Ref} from "vue";
import {Dialog, DialogOverlay, DialogTitle, TransitionChild, TransitionRoot} from "@headlessui/vue";
import ButtonStyle from "~/types/ButtonStyle";

const props = defineProps({
    clusterId: {
        type: String,
        required: true
    },
    clusterName: {
        type: String,
        required: true
    }
})

const emit = defineEmits(["cluster-deleted"]);
const httpClient = useClustersHttpClient();

const modalOpened: Ref<boolean> = ref(false);

async function delete_() {
    await httpClient.delete_(props.clusterId!);
    closeModal();
    emit("cluster-deleted");
}

function openModal() {
    modalOpened.value = true;
}

function closeModal() {
    modalOpened.value = false;
}
</script>