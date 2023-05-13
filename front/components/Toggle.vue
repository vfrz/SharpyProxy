<template>
    <SwitchGroup as="div" class="flex items-center">
        <Switch v-model="enabled"
                :class="[enabled ? 'bg-primary-500' : 'bg-slate-200', 'relative inline-flex flex-shrink-0 h-6 w-11 border-2 border-transparent rounded-full cursor-pointer transition-colors ease-in-out duration-200']">
            <span aria-hidden="true"
                  :class="[enabled ? 'translate-x-5' : 'translate-x-0', 'pointer-events-none inline-block h-5 w-5 rounded-full bg-white shadow transform ring-0 transition ease-in-out duration-200']"/>
        </Switch>
        <SwitchLabel as="span" class="ml-2">
            <slot></slot>
        </SwitchLabel>
    </SwitchGroup>
</template>

<script setup lang="ts">
import {Switch, SwitchGroup, SwitchLabel} from "@headlessui/vue";

const enabled = defineModel<boolean>();
</script>