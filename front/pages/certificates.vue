<template>
    <Title>SharpyProxy - Certificates</Title>
    <Container>
        <div class="sm:flex sm:items-center">
            <div class="sm:flex-auto">
                <MainTitle>
                    Certificates ({{ certificates?.length ?? 0 }})
                </MainTitle>
                <p class="mt-2 text-sm text-slate-700">
                    A list of all the certificates on your SharpyProxy instance.
                </p>
            </div>
            <div class="mt-4 sm:mt-0 sm:ml-16 sm:flex-none">
                <Button type="button">
                    Upload certificate
                </Button>
            </div>
        </div>
        <div class="flex flex-col mt-4">
            <div class="-my-2 -mx-4 overflow-x-auto sm:-mx-6 lg:-mx-8">
                <div class="inline-block min-w-full py-2 align-middle md:px-6 lg:px-8">
                    <div class="overflow-hidden shadow ring-1 ring-black ring-opacity-5 md:rounded-lg">
                        <table class="min-w-full divide-y divide-slate-300">
                            <thead class="bg-slate-50">
                            <tr>
                                <th scope="col"
                                    class="py-3.5 px-4 pr-3 text-left text-sm font-semibold text-slate-900 sm:pl-6">
                                    Name
                                </th>
                                <th scope="col" class="px-3 py-3.5 text-left text-sm font-semibold text-slate-900">
                                    Domain
                                </th>
                                <th scope="col" class="px-3 py-3.5 text-left text-sm font-semibold text-slate-900">
                                    Expiration
                                </th>
                                <th scope="col" class="relative py-3.5 pl-3 pr-4 sm:pr-6">
                                    <span class="sr-only">Edit</span>
                                </th>
                            </tr>
                            </thead>
                            <tbody class="divide-y divide-gray-200 bg-white">
                            <tr v-for="certificate in certificates" :key="certificate.id">
                                <td class="whitespace-nowrap py-4 pl-4 pr-3 text-sm sm:pl-6 font-medium"
                                    :title="certificate.id">
                                    {{ certificate.name }}
                                </td>
                                <td class="whitespace-nowrap px-3 py-4 text-sm text-slate-500">
                                    {{ certificate.domain }}
                                </td>
                                <td class="whitespace-nowrap px-3 py-4 text-sm text-slate-500">
                                    {{ certificate.expiration }}
                                </td>
                                <td class="relative whitespace-nowrap py-4 pl-3 pr-4 text-right text-sm font-semibold sm:pr-6">
                                    <NuxtLink to="#" class="text-primary-500 hover:text-primary-800">
                                        Edit
                                    </NuxtLink>
                                </td>
                            </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </Container>
</template>

<script setup lang="ts">
import {Ref} from "vue";
import ListCertificateModel from "~/models/certificate/ListCertificateModel";
import EnabledTag from "~/components/EnabledTag.vue";

const httpClient = useCertificatesHttpClient();

const certificates: Ref<ListCertificateModel[]> = ref([]);

onNuxtReady(async () => {
    await reloadCertificates();
});

async function reloadCertificates() {
    certificates.value = await httpClient.list();
}
</script>