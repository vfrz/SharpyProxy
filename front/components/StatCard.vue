<template>
    <div class="px-4 py-5 bg-white shadow rounded-lg overflow-hidden sm:p-6">
        <dt class="text-sm font-medium text-slate-500 truncate">
            {{ props.title }}
        </dt>
        <dd class="mt-1 text-3xl font-semibold text-slate-900">
            {{ props.value }}
        </dd>
    </div>
</template>

<script setup lang="ts">
const props = defineProps({
    title: {
        type: String,
        required: true
    },
    value: {
        type: String,
        required: true
    }
})
</script>