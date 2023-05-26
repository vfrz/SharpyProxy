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
      <TabPanels class="mt-2">
        <TabPanel>
          <div class="text-sm text-slate-500 italic">
            Managed certificates are generated and renewed automatically by SharpyProxy using Let's Encrypt;
            by using it you accept their terms of use.
          </div>
          <Form v-if="managedFormModel" @submit="saveManaged">
            <div class="block text-sm font-semibold text-slate-700 mt-2">
              Certificate name
            </div>
            <div class="flex gap-x-4 items-center mt-1">
              <div class="grow">
                <Field type="text"
                       name="name"
                       v-model="managedFormModel.name"
                       placeholder="Name (eg: certificate-1)"
                       class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
              </div>
            </div>
            <div class="block text-sm font-semibold text-slate-700 mt-4">
              Domain
            </div>
            <div class="flex gap-x-4 items-center mt-1">
              <div class="grow">
                <Field type="text"
                       name="domain"
                       v-model="managedFormModel.domain"
                       placeholder="Domain (eg: sharpyproxy.dev)"
                       class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
              </div>
            </div>
            <div class="block text-sm font-semibold text-slate-700 mt-4">
              Email address
            </div>
            <div class="flex gap-x-4 items-center mt-1">
              <div class="grow">
                <Field type="text"
                       name="email"
                       v-model="managedFormModel.email"
                       placeholder="Email (eg: contact@sharpyproxy.dev)"
                       class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
              </div>
            </div>
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
          <div class="text-sm text-slate-500 italic text-jus">
            Unmanaged certificates are your own responsibility and they can't be renewed automatically.
            We advise you to use them unless necessary.
          </div>
          <Form v-if="unmanagedFormModel" @submit="saveUnmanaged">
            <div class="block text-sm font-semibold text-slate-700 mt-2">
              Certificate name
            </div>
            <div class="flex gap-x-4 items-center mt-1">
              <div class="grow">
                <Field type="text"
                       name="name"
                       v-model="unmanagedFormModel.name"
                       placeholder="Name (eg: certificate-1)"
                       class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
              </div>
            </div>
            <div class="block text-sm font-semibold text-slate-700 mt-4">
              Certificate PEM
            </div>
            <div class="flex gap-x-4 items-center mt-1">
              <div class="grow">
                <Field v-slot="{ field, errors }" name="pem" v-model="unmanagedFormModel.pem">
                  <textarea v-bind="field"
                            rows="4"
                            placeholder="-----BEGIN CERTIFICATE-----
...
-----END CERTIFICATE-----"
                            class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
                </Field>
              </div>
            </div>
            <div class="block text-sm font-semibold text-slate-700 mt-4">
              Certificate RSA Key PEM
            </div>
            <div class="flex gap-x-4 items-center mt-1">
              <div class="grow">
                <Field v-slot="{ field, errors }" name="key" v-model="unmanagedFormModel.key">
                  <textarea v-bind="field"
                            rows="4"
                            placeholder="-----BEGIN RSA PRIVATE KEY-----
...
-----END RSA PRIVATE KEY-----"
                            class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
                </Field>
              </div>
            </div>
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
import {reactive, ref, Ref} from "vue";
import ModalTitle from "~/components/ModalTitle.vue";
import Separator from "~/components/Separator.vue";
import ButtonStyle from "~/types/ButtonStyle";
import {Tab, TabGroup, TabList, TabPanel, TabPanels} from "@headlessui/vue";
import CreateClusterModel from "~/models/cluster/CreateClusterModel";
import CreateManagedCertificateModel from "~/models/certificate/CreateManagedCertificateModel";
import UploadCertificateModel from "~/models/certificate/UploadCertificateModel";

const emit = defineEmits(["certificate-created"]);

const httpClient = useCertificatesHttpClient();
const modalOpened: Ref<boolean> = ref(false);

const managedFormModel: CreateManagedCertificateModel = reactive({
  name: "",
  domain: "",
  email: ""
});

async function saveManaged() {
  await httpClient.createManaged(managedFormModel);
  emit("certificate-created");
}

const unmanagedFormModel: UploadCertificateModel = reactive({
  name: "",
  key: "",
  pem: ""
});

async function saveUnmanaged() {
  await httpClient.upload(unmanagedFormModel);
  emit("certificate-created")
}

function openModal() {
  modalOpened.value = true;
}

function closeModal() {
  modalOpened.value = false;
}
</script>