<template>
  <button :disabled="loading || disabled"
          class="flex items-center justify-center py-2 px-4 shadow-sm text-sm font-bold rounded-md border disabled:pointer-events-none"
          :class=classes>
    <svg v-if="props.loading" class="w-4 h-4 mr-2 -ml-1 text-white animate-spin" xmlns="http://www.w3.org/2000/svg"
         fill="none"
         viewBox="0 0 24 24">
      <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
      <path class="opacity-75" fill="currentColor"
            d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z">
      </path>
    </svg>
    <slot></slot>
  </button>
</template>

<script setup lang="ts">
import ButtonStyle from "~/types/ButtonStyle";
import {ComputedRef} from "vue";

const props = defineProps({
    style: {
        type: String as () => ButtonStyle,
        default: ButtonStyle.Primary,
        required: false
    },
    loading: {
        type: Boolean,
        default: false,
        required: false
    },
    disabled: {
        type: Boolean,
        default: false,
        required: false
    }
})

const classes: ComputedRef<string> = computed(() => {
    switch (props.style) {
        case ButtonStyle.DangerOutline:
            return "border-danger-500 text-danger-500 hover:bg-danger-500 hover:text-white";
        case ButtonStyle.Primary:
        default:
            return "border-transparent text-white bg-primary-500 hover:bg-primary-600";
    }
});
</script>