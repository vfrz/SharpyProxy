<template>
    <span :class="[props.enabled ? 'text-green-800 bg-green-100' : 'text-red-800 bg-red-100', 'inline-flex rounded-full px-2 text-xs font-semibold leading-5']">
        {{ props.enabled ? 'Enabled' : 'Disabled' }}
    </span>
</template>

<script setup lang="ts">
const props = defineProps({
    enabled: {
        type: Boolean,
        required: true
    }
})
</script>