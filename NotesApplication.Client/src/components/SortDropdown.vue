<script setup lang="ts">
import { computed } from 'vue'
import type { SortDirection, SortField } from '@/types/note'

const props = defineProps<{
  sortField: SortField
  sortDirection: SortDirection
}>()

const emit = defineEmits<{
  change: [field: SortField, direction: SortDirection]
}>()

const options: { value: string; label: string; field: SortField; direction: SortDirection }[] = [
  { value: 'updatedAt-desc', label: 'Recently updated', field: 'updatedAt', direction: 'desc' },
  { value: 'createdAt-desc', label: 'Newest first', field: 'createdAt', direction: 'desc' },
  { value: 'createdAt-asc', label: 'Oldest first', field: 'createdAt', direction: 'asc' },
  { value: 'title-asc', label: 'Title, A → Z', field: 'title', direction: 'asc' },
  { value: 'title-desc', label: 'Title, Z → A', field: 'title', direction: 'desc' },
]

const selected = computed(() => `${props.sortField}-${props.sortDirection}`)

function onChange(event: Event) {
  const value = (event.target as HTMLSelectElement).value
  const found = options.find((o) => o.value === value)
  if (found) emit('change', found.field, found.direction)
}
</script>

<template>
  <label class="flex items-center gap-2">
    <span class="font-mono text-[11px] uppercase tracking-widest text-ink-faint">Sort</span>
    <select
      :value="selected"
      class="rounded-sm border border-line bg-card py-2 px-2 text-sm text-ink focus:border-stamp"
      @change="onChange"
    >
      <option v-for="opt in options" :key="opt.value" :value="opt.value">{{ opt.label }}</option>
    </select>
  </label>
</template>
