<template>
  <div>
    <slot />
    <v-btn
      :loading="reconcileBusy"
      class="summary-btn elevation-10"
      small
      fixed
      @click="returnToSummary"
      >{{ buttonText }}</v-btn
    >
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
    returnToSummary() {
      this.$store.dispatch('Reconcile/getReconcileSummary', () => {
        this.$emit('buttonClicked')
      })
    }
  },
  computed: {
    reconcileBusy() {
      return this.$store.state.Reconcile.reconcileBusy
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
