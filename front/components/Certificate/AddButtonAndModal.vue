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
        <Tab :disabled="loading" v-slot="{ selected }" class="grow disabled:pointer-events-none">
          <div class="py-2 px-1 text-center border-b-2 font-medium"
               :class="selected ? 'border-primary-500 text-primary-600' : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'">
            Managed
          </div>
        </Tab>
        <Tab :disabled="loading" v-slot="{ selected }" class="grow disabled:pointer-events-none">
          <div class="py-2 px-1 text-center border-b-2 font-medium"
               :class="selected ? 'border-primary-500 text-primary-600' : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'">
            Unmanaged
          </div>
        </Tab>
      </TabList>
      <TabPanels class="mt-2">
        <TabPanel>
          <div class="text-sm text-slate-500 italic">
            Managed certificates are generated and renewed automatically by SharpyProxy using Let's Encrypt;
            by using it you accept their
            <NuxtLink class="underline" to="https://letsencrypt.org/repository/" target="_blank">terms of use</NuxtLink>
            .
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
                       :disabled="loading"
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
                       :disabled="loading"
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
                       :disabled="loading"
                       placeholder="Email (eg: contact@sharpyproxy.dev)"
                       class="shadow-sm focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
              </div>
            </div>
            <div class="grid grid-cols-2 mt-4 gap-x-2">
              <Button :loading="loading" type="submit">
                {{ loading ? "Generating certificate..." : "Save" }}
              </Button>
              <Button :disabled="loading" type="button" @click="closeModal"
                      :style="ButtonStyle.RedOutline">
                Cancel
              </Button>
            </div>
          </Form>
        </TabPanel>
        <TabPanel>
          <div class="text-sm text-slate-500 italic text-jus">
            Unmanaged certificates are your own responsibility and they can't be renewed automatically.
            We discourage you to use them unless necessary.
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
                       :disabled="loading"
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
                            :disabled="loading"
                            rows="5"
                            placeholder="-----BEGIN CERTIFICATE-----
...
-----END CERTIFICATE-----"
                            class="shadow-sm resize-none focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
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
                            :disabled="loading"
                            rows="5"
                            placeholder="-----BEGIN RSA PRIVATE KEY-----
...
-----END RSA PRIVATE KEY-----"
                            class="shadow-sm resize-none focus:ring-primary-500 focus:border-primary-500 block w-full sm:text-sm border-slate-300 rounded-md"/>
                </Field>
              </div>
            </div>
            <div class="grid grid-cols-2 mt-4 gap-x-2">
              <Button :loading="loading" type="submit">
                {{ loading ? "Saving certificate..." : "Save" }}
              </Button>
              <Button :disabled="loading" type="button" @click="closeModal"
                      :style="ButtonStyle.RedOutline">
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
import ButtonStyle from "~/types/ButtonStyle";
import {Tab, TabGroup, TabList, TabPanel, TabPanels} from "@headlessui/vue";
import CreateManagedCertificateModel from "~/models/certificate/CreateManagedCertificateModel";
import UploadCertificateModel from "~/models/certificate/UploadCertificateModel";
import {def} from "@vue/shared";

const emit = defineEmits(["certificate-created"]);

const httpClient = useCertificatesHttpClient();
const modalOpened: Ref<boolean> = ref(false);
const loading: Ref<boolean> = ref(false);

const emptyManagedFormModel: CreateManagedCertificateModel = {
    name: "",
    domain: "",
    email: ""
};
const managedFormModel: CreateManagedCertificateModel = reactive({...emptyManagedFormModel});

async function saveManaged() {
    loading.value = true;
    await httpClient.createManaged(managedFormModel);
    closeModal();
    loading.value = false;
    emit("certificate-created");
}

const emptyUnmanagedFormModel: UploadCertificateModel = {
    name: "",
    key: "",
    pem: ""
}
const unmanagedFormModel: UploadCertificateModel = reactive({...emptyUnmanagedFormModel});

async function saveUnmanaged() {
    loading.value = true;
    await httpClient.upload(unmanagedFormModel);
    closeModal();
    loading.value = false;
    emit("certificate-created");
}

function openModal() {
    Object.assign(managedFormModel, emptyManagedFormModel);
    Object.assign(unmanagedFormModel, emptyUnmanagedFormModel);
    modalOpened.value = true;
}

function closeModal() {
    modalOpened.value = false;
}
</script>