<template>
    <div>
        <dl class="mt-4 grid grid-cols-1 gap-5 sm:grid-cols-3">
            <StatCard title="Clusters" :value="stats.clusterCount.toString()"/>
            <StatCard title="Routes" :value="stats.routeCount.toString()"/>
            <StatCard title="Certificates" :value="stats.certificateCount.toString()"/>
        </dl>
    </div>
</template>

<script setup lang="ts">
import StatCard from "~/components/StatCard.vue";

const coreHttpClient = useCoreHttpClient();
const stats = await coreHttpClient.getStats();
</script>