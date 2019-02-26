<template>
  <div>
    <slot />
    <v-btn class="summary-btn elevation-10" small fixed @click="notifyParent">{{
      buttonText
    }}</v-btn>
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

<style scoped>
.summary-btn {
  z-index: 2;
  top: 13px;
  left: 124px;
}
</style>
