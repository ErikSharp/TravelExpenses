<template>
  <v-card
    light
    class="my-1"
    @click="selectTransaction()"
    :class="{ selected: transactionSelected }"
  >
    <v-flex>
      <v-layout align-center justify-start row fill-height>
        <v-flex shrink>
          <v-avatar size="70" class="mx-2 elevation-5" :class="iconColor">
            <v-icon size="45" class="white--text">{{ icon }}</v-icon>
          </v-avatar>
        </v-flex>
        <v-flex>
          <v-card-text class="white py-1 px-0 border-right">
            <p>
              <strong>Title:</strong>
              {{ transaction.title }}
            </p>
            <p>
              <strong>Category:</strong>
              {{ getCategoryString(transaction.categoryId) }}
            </p>
            <p>
              <strong>Amount:</strong>
              {{
                `${transaction.amount} ${getCurrencyIsoString(
                  transaction.currencyId
                )}`
              }}
            </p>
            <p style="display: inline">
              <strong>Keywords:</strong>
            </p>
            <v-chip small v-for="(id, i) in transaction.keywordIds" :key="i">
              {{ getKeywordName(id) }}
            </v-chip>
          </v-card-text>
        </v-flex>
      </v-layout>
    </v-flex>
  </v-card>
</template>

<script>
export default {
  props: {
    transaction: Object
  },
  created() {
    this.setIcon()
  },
  data() {
    return {
      iconColor: '',
      icon: ''
    }
  },
  methods: {
    selectTransaction() {
      this.$store.dispatch(
        'Transaction/setSelectedTransaction',
        this.transaction
      )
    },
    getCategoryString(id) {
      let category = this.$store.getters['Category/findCategory'](id)
      if (category) {
        return category.categoryName
      }

      return 'unknown'
    },
    getKeywordName(id) {
      let keyword = this.$store.getters['Keyword/findKeyword'](id)
      if (keyword) {
        return keyword.keywordName
      }

      return 'unknown'
    },
    getCurrencyIsoString(id) {
      let currency = this.$store.getters['Currency/findCurrency'](id)
      if (currency) {
        return currency.isoCode
      }

      return ''
    },
    setIcon() {
      let color = Math.floor(Math.random() * 5)
      switch (color) {
        case 0:
          this.iconColor = 'indigo'
          this.icon = 'fastfood'
          break
        case 1:
          this.iconColor = 'purple'
          this.icon = 'shopping_cart'
          break
        case 2:
          this.iconColor = 'green'
          this.icon = 'airplanemode_active'
          break
        case 3:
          this.iconColor = 'red'
          this.icon = 'local_hospital'
          break
        case 4:
          this.iconColor = 'orange'
          this.icon = 'beach_access'
          break
      }
    },
    getIcon() {
      let icon = Math.floor(Math.random() * 5)
      switch (icon) {
        case 0:
          return 'fastfood'
        case 1:
          return 'shopping_cart'
        case 2:
          return 'airplanemode_active'
        case 3:
          return 'local_hospital'
        case 4:
          return 'beach_access'
      }
    }
  },
  computed: {
    transactionSelected() {
      return (
        this.$store.state.Transaction.selectedTransaction == this.transaction
      )
    }
  }
}
</script>

<style scoped>
.v-card p {
  margin: 0;
  height: 22px;
}

.v-chip {
  margin: 0 3px 3px 5px !important;
}

.v-expansion-panel .v-card {
  border-radius: 10px;
}

.border-right {
  border-radius: 10px;
}

.selected {
  background: cornflowerblue;
}

.selected .v-card__text {
  transition: background 0.21s;
  background: cornflowerblue !important;
}
</style>
