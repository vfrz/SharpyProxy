<template>
    <button :class="['py-2 px-4 shadow-sm text-sm font-bold rounded-md border', classes]">
        <slot></slot>
    </button>
</template>

<script setup lang="ts">
import ButtonStyle from "~/types/ButtonStyle";
import {ComputedRef} from "vue";

const classes : ComputedRef<string> = computed(() => {
    switch (props.style) {
        case ButtonStyle.RedOutline:
            return "border-red-500 text-red-500 hover:bg-red-500 hover:text-white";
        case ButtonStyle.Primary:
        default:
            return "border-transparent text-white bg-primary-500 hover:bg-primary-600";
    }
});

const props = defineProps({
    style: {
        type: String as () => ButtonStyle,
        default: ButtonStyle.Primary,
        required: false
    }
})
</script>