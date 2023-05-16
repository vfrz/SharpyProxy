<template>
    <div class="p-5 bg-white shadow rounded-lg flex flex-col gap-y-1">
        <dt class="text-sm font-medium text-slate-500 truncate">
            {{ props.title }}
        </dt>
        <dd class="text-3xl font-semibold text-slate-900">
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