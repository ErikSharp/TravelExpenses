<template>
  <div class="root">
    <v-avatar size="56" tile :color="selected ? 'primary' : 'white'" @click="onIconSelection()">
      <v-avatar :color="iconData.iconColor">
        <v-icon class="white--text">{{ iconData.iconString }}</v-icon>
      </v-avatar>
    </v-avatar>
  </div>
</template>

<script>
export default {
  props: {
    iconData: {
      type: Object,
      required: true
    },
    selectedId: {
      type: Number,
      required: true
    }
  },
  data() {
    return {
      selected: false
    }
  },
  methods: {
    onIconSelection() {
      this.selected = true
      this.$emit('selected', this.iconData)
    }
  },
  watch: {
    selectedId() {
      if (this.selectedId !== this.iconData.id && this.selected) {
        this.selected = false
      }
    }
  }
}
</script>

<style scoped>
.root > .v-avatar {
  border-radius: 5px;
}
</style>
