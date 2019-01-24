<template>
  <v-card class="pa-3">
    <v-flex xs12 class="mb-3">
      <v-layout row align-center>
        <v-avatar size="55" class="mr-3 elevation-4">
          <v-icon large class="primary white--text">collections_bookmark</v-icon>
        </v-avatar>
        <h1>Categories</h1>
      </v-layout>
    </v-flex>
    <p>The following is a pre-defined list of typical Categories you might want to use.</p>
    <p>Refine the selection below to your liking</p>
    <v-select :items="categories" v-model="chosenCategories" label="Categories" chips solo multiple>
      <template slot="selection" slot-scope="data">
        <v-chip :selected="data.selected" close @input="removeCategory(data.item)">
          <span>{{ data.item }}</span>
        </v-chip>
      </template>
      <template slot="item" slot-scope="data">
        <v-checkbox
          v-model="chosenCategories"
          color="primary"
          :value="data.item"
          :label="data.item"
        />
      </template>
    </v-select>
    <p>
      <strong>Note:</strong> You will be able to add additional Categories by using the Setup menu within the application.
    </p>
    <v-flex xs12>
      <v-layout row justify-center>
        <v-btn
          :loading="busy"
          :disabled="!chosenCategories.length"
          color="primary"
          @click="next"
        >NEXT</v-btn>
      </v-layout>
    </v-flex>
  </v-card>
</template>

<script>
import Windows from '@/common/enums/InitialSetupWindows.js'
import clone from 'lodash/clone'

export default {
  created() {
    this.chosenCategories = clone(this.categories)
  },
  data() {
    return {
      chosenCategories: []
    }
  },
  methods: {
    next() {
      this.$store
        .dispatch('Category/addCategories', this.chosenCategories)
        .then(() => {
          this.$store.dispatch('InitialSetup/setWindow', this.getNextWindow())
        })
    },
    getNextWindow() {
      if (!this.$store.state.InitialSetup.baseData.hasKeyword) {
        return Windows.keywords1
      } else {
        return Windows.finish
      }
    },
    removeCategory(item) {
      this.chosenCategories.splice(this.chosenCategories.indexOf(item), 1)
      this.chosenCategories = [...this.chosenCategories]
    }
  },
  computed: {
    categories() {
      return this.$store.state.Category.sampleCategories
    },
    busy() {
      return this.$store.state.Category.addCategoryBusy
    }
  }
}
</script>

<style scoped>
</style>
