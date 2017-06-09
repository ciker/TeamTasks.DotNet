import { Factory } from 'ember-cli-mirage';

function makeUserName (i) {
  let startingNames = ['Izzy', 'Don', 'Andrey'];
  return i < 3 ? startingNames[i] : 'AdditionalUser' + (i-2);
}

function makeEmail (i) {
  let startingEmail = ['mr.i.d.fernando@gmail.com', 'don@happycollision.com', 'andrey@flixpress.com'];
  return i < 3 ? startingEmail[i] : 'AdditionalUser' + (i-2) + '@example.com';
}

export default Factory.extend({
  id(i) {return ++i;},
  name: makeUserName,
  email: makeEmail
});
