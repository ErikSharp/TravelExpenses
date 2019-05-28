<template>
  <v-dialog v-model="infoDialog" max-width="290" persistent>
    <template v-slot:activator="{ on }">
      <v-btn small color="primary" outline v-on="on">{{ title }}</v-btn>
    </template>
    <v-card>
      <v-avatar size="70" class="mx-2 elevation-5" :class="color">
        <v-icon size="45" class="white--text">{{ icon }}</v-icon>
      </v-avatar>
      <v-card-title class="title headline pb-0">{{ title }}</v-card-title>
      <v-card-text class="subheading">
        <slot />
      </v-card-text>
      <v-card-actions>
        <v-btn class="mx-auto" color="primary" @click="infoDialog = false"
          >CLOSE</v-btn
        >
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  props: {
    color: {
      type: String,
      required: true
    },
    title: {
      type: String,
      required: true
    },
    icon: {
      type: String,
      default: 'warning'
    }
  },
  data() {
    return {
      infoDialog: false
    }
  }
}
</script>

<style lang="scss" scoped>
.title {
  position: absolute;
  top: 5px;
  left: 80px;
}
</style>