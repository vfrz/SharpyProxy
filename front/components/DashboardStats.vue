<template>
    <div>
        <dl class="mt-4 grid grid-cols-1 gap-5 sm:grid-cols-3">
            <div v-for="item in stats" :key="item.name"
                 class="px-4 py-5 bg-white shadow rounded-lg overflow-hidden sm:p-6">
                <dt class="text-sm font-medium text-slate-500 truncate">
                    {{ item.name }}
                </dt>
                <dd class="mt-1 text-3xl font-semibold text-slate-900">
                    {{ item.stat }}
                </dd>
            </div>
        </dl>
    </div>
</template>

<script setup lang="ts">
const stats = [
    {name: 'Clusters', stat: '42'},
    {name: 'Routes', stat: '34'},
    {name: 'Certificates', stat: '16'},
]
</script>