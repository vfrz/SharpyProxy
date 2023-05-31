<template>
  <TransitionRoot
      :show="opened"
      as="template">
    <Dialog class="relative z-10">
      <TransitionChild as="template" enter="ease-out duration-300" enter-from="opacity-0" enter-to="opacity-100"
                       leave="ease-in duration-200" leave-from="opacity-100" leave-to="opacity-0">
        <DialogOverlay class="fixed inset-0 bg-slate-500 bg-opacity-75 transition-opacity"/>
      </TransitionChild>
      <div class="fixed inset-0 flex justify-center mt-8">
        <TransitionChild as="template" enter="ease-out duration-300"
                         enter-from="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                         enter-to="opacity-100 translate-y-0 sm:scale-100" leave="ease-in duration-200"
                         leave-from="opacity-100 translate-y-0 sm:scale-100"
                         leave-to="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95">
          <DialogPanel class="relative w-full h-fit p-4 max-w-xl rounded-lg bg-white">
            <slot></slot>
          </DialogPanel>
        </TransitionChild>
      </div>
    </Dialog>
  </TransitionRoot>
</template>

<script setup lang="ts">
import {Dialog, DialogOverlay, DialogPanel, TransitionChild, TransitionRoot} from "@headlessui/vue";

const props = defineProps({
    opened: Boolean
});
</script>