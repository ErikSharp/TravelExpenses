<template>
  <div>
    <slot />
    <v-btn @click="notifyParent">{{ buttonText }}</v-btn>
  </div>
</template>

<script>
export default {
  props: {
    buttonText: {
      type: String,
      default: 'Cancel'
    }
  },
  methods: {
    notifyParent() {
      this.$emit('buttonClicked')
    }
  }
}
</script>

<style scoped></style>
