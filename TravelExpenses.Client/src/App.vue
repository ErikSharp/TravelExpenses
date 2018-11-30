<template>
  <div id="app">
    <v-app>
      <v-content>
        <transition name="fade">
          <router-view/>
        </transition>
      </v-content>
    </v-app>
  </div>
</template>

<script>
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
</style>
