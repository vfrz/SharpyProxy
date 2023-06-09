<template>
  <Title>SharpyProxy - Routes</Title>
  <Container>
    <div class="sm:flex sm:items-center">
      <div class="sm:flex-auto">
        <MainTitle>
          Routes ({{ routes?.length ?? 0 }})
        </MainTitle>
        <p class="mt-2 text-sm text-slate-700">
          A list of all the routes on your SharpyProxy instance.
        </p>
      </div>
      <div class="mt-4 sm:mt-0 sm:ml-16 sm:flex-none">
        <RouteAddButtonAndModal @route-created="reloadRoutes"></RouteAddButtonAndModal>
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
                  Cluster
                </th>
                <th scope="col" class="px-3 py-3.5 text-left text-sm font-semibold text-slate-900">
                  Match hosts
                </th>
                <th scope="col" class="px-3 py-3.5 text-left text-sm font-semibold text-slate-900">
                  Match path
                </th>
                <th scope="col" class="px-3 py-3.5 text-left text-sm font-semibold text-slate-900">
                  Status
                </th>
                <th scope="col" class="relative py-3.5 pl-3 pr-4 sm:pr-6">
                  <span class="sr-only">Edit</span>
                </th>
              </tr>
              </thead>
              <tbody class="divide-y divide-gray-200 bg-white">
              <tr v-for="route in routes.sort((a, b) => a.name.localeCompare(b.name))" :key="route.id">
                <td class="whitespace-nowrap py-4 pl-4 pr-3 text-sm sm:pl-6 font-medium" :title="route.id">
                  {{ route.name }}
                </td>
                <td class="whitespace-nowrap px-3 py-4 text-sm text-slate-500" :title="route.clusterId">
                  <div class="flex items-center">
                    <span
                        :class="[route.clusterEnabled ? 'bg-green-400' : 'bg-orange-400', 'flex-shrink-0 inline-block h-2 w-2 rounded-full']"
                        aria-hidden="true"/>
                    <span class="ml-2 block truncate">
                      {{ route.clusterName }}
                    </span>
                  </div>
                </td>
                <td class="whitespace-nowrap px-3 py-4 text-sm text-slate-500">
                  {{ route.matchHosts.join(", ") }}
                </td>
                <td class="whitespace-nowrap px-3 py-4 text-sm text-slate-500">
                  {{ route.matchPath }}
                </td>
                <td class="whitespace-nowrap px-3 py-4 text-sm text-slate-500">
                  <EnabledTag :enabled="route.enabled"/>
                </td>
                <td class="relative whitespace-nowrap py-4 pl-3 pr-4 text-right text-sm font-semibold sm:pr-6">
                  <div class="inline-flex gap-x-4">
                    <RouteEditButtonAndModal :route-id="route.id" @route-updated="reloadRoutes"/>
                    <RouteDeleteButtonAndModal :route-id="route.id" :route-name="route.name" @route-deleted="reloadRoutes"/>
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
import EnabledTag from "~/components/EnabledTag.vue";
import {Ref} from "vue";
import ListRouteModel from "~/models/route/ListRouteModel";

const httpClient = useRoutesHttpClient();

const routes: Ref<ListRouteModel[]> = ref([]);

onNuxtReady(async () => {
    await reloadRoutes();
});

async function reloadRoutes() {
    routes.value = await httpClient.list();
}

</script>