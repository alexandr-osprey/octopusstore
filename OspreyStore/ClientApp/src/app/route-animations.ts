import {
  trigger,
  transition,
  style,
  query,
  group,
  animateChild,
  animate,
  keyframes,
} from '@angular/animations';

//export const slideInAnimation =

    //transition('HomePage <=> AboutPage', [
    //  style({ position: 'relative' }),
    //  query(':enter, :leave', [
    //    style({
    //      position: 'absolute',
    //      top: 0,
    //      left: 0,
    //      width: '100%'
    //    })
    //  ]),
    //  query(':enter', [
    //    style({ left: '-100%' })
    //  ]),
    //  query(':leave', animateChild()),
    //  group([
    //    query(':leave', [
    //      animate('300ms ease-out', style({ left: '100%' }))
    //    ]),
    //    query(':enter', [
    //      animate('300ms ease-out', style({ left: '0%' }))
    //    ])
    //  ]),
    //  query(':enter', animateChild()),
    //]),
//  trigger('routeAnimations', [
  //  transition('* => *', [
  //    style({ position: 'relative' }),
  //    query(':enter, :leave', [
  //      style({
  //        position: 'absolute',
  //        top: 0,
  //        left: 0,
  //        width: '100%'
  //      })
  //    ]),
  //    query(':enter', [
  //      style({ left: '-100%' })
  //    ]),
  //    query(':leave', animateChild()),
  //    group([
  //      query(':leave', [
  //        animate('200ms ease-out', style({ left: '100%' }))
  //      ]),
  //      query(':enter', [
  //        animate('300ms ease-out', style({ left: '0%' }))
  //      ])
  //    ]),
  //    query(':enter', animateChild()),
  //  ])
  //]);
export const optional = { optional: true };
export const slideInAnimation =
  trigger('routeAnimations', [
    transition('* => *', [
      query(':enter, :leave', [
        style({ position: 'relative', })
      ], optional),
      query(':enter', [
        style({ left: '-100%' })
      ], optional),
      query(':leave', animateChild(), optional),
      group([
        query(':leave', [
          animate('300ms ease-out', style({ left: '100%' }))
        ], optional),
        query(':enter', [
          animate('400ms ease-out', style({ left: '0%' }))
        ], optional)
      ]),
      query(':enter', animateChild(), optional),
    ]),
    //transition('* => isRight', slideTo('right')),
    //transition('isRight => *', slideTo('left')),
    //transition('isLeft => *', slideTo('right'))
  ]);

export function slideTo(direction) {
  
  return [
    
    // Normalize the page style... Might not be necessary

    // Required only if you have child animations on the page
     //query(':leave', animateChild()),
     //query(':enter', animateChild()),
  ];
}
  trigger('routeAnimations', [
    transition('* => *', [
      style({ position: 'relative' }),
      query(':enter, :leave', [
        style({
          position: 'absolute',
          top: 0,
          left: 0,
          width: '100%'
        })
      ]),
      query(':enter', [
        style({ left: '-100%' })
      ]),
      query(':leave', animateChild()),
      group([
        query(':leave', [
          animate('200ms ease-out', style({ left: '100%' }))
        ]),
        query(':enter', [
          animate('300ms ease-out', style({ left: '0%' }))
        ])
      ]),
      query(':enter', animateChild()),
    ])
  ]);
