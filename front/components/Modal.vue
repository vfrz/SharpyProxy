<template>
  <TransitionRoot
      :show="opened"
      as="template"
      enter="duration-200 ease-out"
      enter-from="opacity-0"
      enter-to="opacity-100"
      leave="duration-200 ease-in"
      leave-from="opacity-100"
      leave-to="opacity-0">
    <Dialog class="relative z-40">
      <TransitionChild as="template" enter="ease-out duration-300" enter-from="opacity-0" enter-to="opacity-100"
                       leave="ease-in duration-200" leave-from="opacity-100" leave-to="opacity-0">
        <DialogOverlay class="fixed inset-0 bg-slate-800 bg-opacity-80 transition-opacity"/>
      </TransitionChild>

      <div class="fixed inset-0 flex justify-center mt-8">
        <DialogPanel class="relative w-full h-fit p-4 max-w-xl rounded-lg bg-white">
          <slot></slot>
        </DialogPanel>
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