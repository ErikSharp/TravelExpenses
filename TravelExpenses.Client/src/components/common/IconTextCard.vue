<template>
  <v-card
    :class="{ active: active, inactive: !active }"
    class="my-1"
    @click="onClick"
  >
    <v-flex>
      <v-layout align-center justify-start row fill-height>
        <v-flex shrink>
          <v-avatar
            size="60"
            class="mx-3 my-2 elevation-5"
            :color="active ? color : 'grey'"
          >
            <v-icon size="40" class="white--text">{{ icon }}</v-icon>
          </v-avatar>
        </v-flex>
        <v-flex class="mr-3 mt-2 mb-2">
          <div class="subheading font-weight-bold mb-1">{{ title }}</div>
          <span>
            <slot />
          </span>
        </v-flex>
      </v-layout>
    </v-flex>
  </v-card>
</template>

<script>
export default {
  props: {
    color: {
      type: String,
      default: 'primary'
    },
    icon: {
      type: String,
      required: true
    },
    title: {
      type: String,
      required: true
    },
    active: {
      type: Boolean,
      default: true
    }
  },
  methods: {
    onClick() {
      if (this.active) {
        this.$emit('click')
      }
    }
  }
}
</script>

<style scoped>
.inactive {
  background: rgba(255, 255, 255, 0.3);
}

.active {
  background: white;
}
</style>