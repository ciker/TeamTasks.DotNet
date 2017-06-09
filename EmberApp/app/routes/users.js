import Ember from 'ember';

export default Ember.Route.extend({
  model() {
    return [
      {
        id: 1,
        name: 'Andrey',
        email: 'example@example.com'
      }, 
      {
        id: 2,
        name: 'Izzy',
        email: 'example@example.com'
      }, 
      {
        id: 3,
        name: 'Don',
        email: 'example@example.com'
      }
    ];
  }
});
