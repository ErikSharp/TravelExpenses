<template>
  <div>
    <v-select
      :items="currencies"
      v-model="currency"
      return-object
      box
      background-color="white"
      color="primary"
      label="Select Display Currency"
    >
      <template slot="selection" slot-scope="data">
        <div>
          <span>
            <strong>{{ data.item.isoCode }}</strong>
            - {{ data.item.currencyName }}
          </span>
        </div>
      </template>
      <template slot="item" slot-scope="data">
        <div>
          <span>
            <strong>{{ data.item.isoCode }}</strong>
            - {{ data.item.currencyName }}
          </span>
        </div>
      </template>
    </v-select>
    <query-card
      color="blue"
      icon="assessment"
      title="Locations total comparison"
      class="mb-2"
      @click="navLocTotalComp"
    >
      Compares the total amount spent in each location
    </query-card>
    <query-card
      color="purple"
      icon="assessment"
      title="Locations categories comparison"
      @click="navLocCatComp"
    >
      Compares categories in each location
    </query-card>
  </div>
</template>

<script>
import QueryCard from '@/components/query/QueryCard.vue'
import QueryWindows from '@/common/enums/QueryWindows.js'
import { mapState } from 'vuex'

export default {
  components: {
    QueryCard
  },
  methods: {
    navLocTotalComp() {
      this.$store.dispatch('Query/setQueryWindow', QueryWindows.locTotalComp)
    },
    navLocCatComp() {
      this.$store.dispatch('Query/setQueryWindow', QueryWindows.locCatComp)
    }
  },
  computed: {
    ...mapState('Currency', ['currencies']),
    currency: {
      get() {
        return this.$store.state.Query.currency
      },
      set(val) {
        this.$store.dispatch('Query/setCurrency', val)
      }
    }
  }
}
</script>

<style scoped></style>
