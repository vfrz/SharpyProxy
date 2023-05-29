<template>
  <div class="grid grid-cols-1 sm:grid-cols-3 gap-x-5 gap-y-3">
    <StatCard title="Clusters" :value="stats.clusterCount.toString()"/>
    <StatCard title="Routes" :value="stats.routeCount.toString()"/>
    <StatCard title="Certificates" :value="stats.certificateCount.toString()"/>
  </div>
</template>

<script setup lang="ts">
import StatCard from "~/components/StatCard.vue";

const coreHttpClient = useCoreHttpClient();
const stats = await coreHttpClient.getStats();
</script>