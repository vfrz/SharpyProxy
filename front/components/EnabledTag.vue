<template>
    <span
        :class="[props.enabled ? 'text-green-700 bg-green-100' : 'text-orange-700 bg-orange-100', 'inline-flex rounded-full px-2 text-xs font-semibold leading-5']">
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