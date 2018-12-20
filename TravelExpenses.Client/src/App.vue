<template>
  <div id="app">
    <v-app>
      <v-content>
        <transition name="fade">
          <router-view/>
        </transition>
        <v-snackbar
          v-model="snackbar.show"
          :color="snackbar.color"
          :multi-line="snackbar.mode === 'multi-line'"
          :timeout="snackbar.timeout"
          :vertical="snackbar.mode === 'vertical'"
          top
        >
          {{ snackbar.message }}
          <!-- <v-btn
            dark
            flat
            @click="snackbar = false"
          >
            Close
          </v-btn>-->
        </v-snackbar>
      </v-content>
    </v-app>
  </div>
</template>

<script>
export default {
  computed: {
    snackbar() {
      return this.$store.state.snackbar
    }
  }
}
</script>

<style lang="scss">
.click-text {
  font-weight: bold;
  cursor: pointer;
  text-transform: uppercase;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.25s;
}
.fade-enter-active {
  transition-delay: 0.25s;
}
.fade-enter, .fade-leave-to /* .fade-leave-active below version 2.1.8 */ {
  opacity: 0;
}

#app {
  background: #261136;
}

.v-text-field--box .v-input__slot {
  border-bottom-left-radius: 4px;
  border-bottom-right-radius: 4px;
}

.v-list {
  border-top-left-radius: 4px;
  border-top-right-radius: 4px;
  border-bottom-left-radius: 4px;
  border-bottom-right-radius: 4px;
  padding-bottom: 0px;
}
</style>
