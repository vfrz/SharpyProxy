<template>
  <Button type="button" @click="openModal">
    Add certificate
  </Button>

  <Modal :opened="modalOpened">
    <ModalTitle class="mb-2">
      Add certificate
    </ModalTitle>
    <TabGroup>
      <TabList class="border-b border-slate-200 flex mb-2">
        <Tab v-slot="{ selected }" class="grow">
          <div
              :class="[selected ? 'border-primary-500 text-primary-600' : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300', 'py-2 px-1 text-center border-b-2 font-medium']">
            Managed
          </div>
        </Tab>
        <Tab v-slot="{ selected }" class="grow">
          <div
              :class="[selected ? 'border-primary-500 text-primary-600' : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300', 'py-2 px-1 text-center border-b-2 font-medium']">
            Unmanaged
          </div>
        </Tab>
      </TabList>
      <TabPanels>
        <TabPanel>
          <Form>
            Managed certificate
            <div class="flex mt-4 gap-x-2">
              <Button type="submit" class="grow">
                Save
              </Button>
              <Button type="button" class="grow" @click="closeModal" :style="ButtonStyle.RedOutline">
                Cancel
              </Button>
            </div>
          </Form>
        </TabPanel>
        <TabPanel>
          <Form>
            Unmanaged certificate
            <div class="flex mt-4 gap-x-2">
              <Button type="submit" class="grow">
                Save
              </Button>
              <Button type="button" class="grow" @click="closeModal" :style="ButtonStyle.RedOutline">
                Cancel
              </Button>
            </div>
          </Form>
        </TabPanel>
      </TabPanels>
    </TabGroup>


  </Modal>
</template>

<script setup lang="ts">
import {ref, Ref} from "vue";
import ModalTitle from "~/components/ModalTitle.vue";
import Separator from "~/components/Separator.vue";
import ButtonStyle from "~/types/ButtonStyle";
import {Tab, TabGroup, TabList, TabPanel, TabPanels} from "@headlessui/vue";

const modalOpened: Ref<boolean> = ref(false);

function openModal() {
  modalOpened.value = true;
}

function closeModal() {
  modalOpened.value = false;
}
</script>