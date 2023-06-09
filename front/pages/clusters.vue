<template>
  <Title>SharpyProxy - Clusters</Title>
  <Container>
    <div class="sm:flex sm:items-center">
      <div class="sm:flex-auto">
        <MainTitle>
          Clusters ({{ clusters?.length ?? 0 }})
        </MainTitle>
        <p class="mt-2 text-sm text-slate-700">
          A list of all the clusters on your SharpyProxy instance.
        </p>
      </div>
      <div class="mt-4 sm:mt-0 sm:ml-16 sm:flex-none">
        <ClusterAddButtonAndModal @cluster-created="reloadClusters"/>
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
                  Destinations
                </th>
                <th scope="col" class="px-3 py-3.5 text-left text-sm font-semibold text-slate-900">
                  Status
                </th>
                <th scope="col" class="relative py-3.5 pl-3 pr-4 sm:pr-6">
                  <span class="sr-only">Actions</span>
                </th>
              </tr>
              </thead>
              <tbody class="divide-y divide-gray-200 bg-white">
              <tr v-for="cluster in clusters?.sort((a, b) => a.name.localeCompare(b.name))" :key="cluster.id">
                <td class="whitespace-nowrap py-4 pl-4 pr-3 text-sm sm:pl-6 font-medium"
                    :title="cluster.id">
                  {{ cluster.name }}
                </td>
                <td class="whitespace-nowrap px-3 py-4 text-sm text-slate-500">
                  {{ cluster.destinations.map(d => d.address).join(", ") }}
                </td>
                <td class="whitespace-nowrap px-3 py-4 text-sm text-slate-500">
                  <EnabledTag :enabled="cluster.enabled"/>
                </td>
                <td class="relative whitespace-nowrap py-4 pl-3 pr-4 text-right text-sm font-semibold sm:pr-6">
                  <div class="inline-flex gap-x-4">
                    <ClusterEditButtonAndModal :cluster-id="cluster.id" @cluster-updated="reloadClusters"/>
                    <ClusterDeleteButtonAndModal :cluster-id="cluster.id" :cluster-name="cluster.name" @cluster-deleted="reloadClusters"/>
                  </div>
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
import ClusterModel from "~/models/cluster/ClusterModel";
import {Ref} from "vue";

const httpClient = useClustersHttpClient();

const clusters: Ref<ClusterModel[]> = ref([]);

onNuxtReady(async () => {
    await reloadClusters();
});

async function reloadClusters() {
    clusters.value = await httpClient.list();
}
</script>