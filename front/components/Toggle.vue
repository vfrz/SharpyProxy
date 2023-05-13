<template>
    <SwitchGroup as="div" class="flex items-center">
        <Switch v-model="enabled"
                :class="[enabled ? 'bg-primary-500' : 'bg-slate-200', 'relative inline-flex flex-shrink-0 h-5 w-10 border-2 border-transparent rounded-full cursor-pointer transition-colors ease-in-out duration-200']">
            <span aria-hidden="true"
                  :class="[enabled ? 'translate-x-5' : 'translate-x-0', 'pointer-events-none inline-block h-4 w-4 rounded-full bg-white shadow transform ring-0 transition ease-in-out duration-200']"/>
        </Switch>
        <SwitchLabel as="span" class="ml-2 cursor-pointer">
            <slot></slot>
        </SwitchLabel>
    </SwitchGroup>
</template>

<script setup lang="ts">
import {Switch, SwitchGroup, SwitchLabel} from "@headlessui/vue";

const enabled = defineModel<boolean>();
</script>