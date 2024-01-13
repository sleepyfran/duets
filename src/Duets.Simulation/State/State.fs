module Duets.Simulation.State.Root

open Aether
open Duets.Entities

/// Applies an effect to the state.
let applyEffect state effect =
    match effect with
    | AlbumStarted(band, unreleasedAlbum) ->
        Albums.addUnreleased band unreleasedAlbum state
        |> Albums.markTrackListAsRecorded band unreleasedAlbum
    | AlbumUpdated(band, unreleasedAlbum) ->
        let (UnreleasedAlbum album) = unreleasedAlbum

        Albums.removeUnreleased band album.Id state
        |> Albums.addUnreleased band unreleasedAlbum
        |> Albums.markTrackListAsRecorded band unreleasedAlbum
    | AlbumReleased(band, releasedAlbum) ->
        let album = releasedAlbum.Album

        Albums.removeUnreleased band album.Id state
        |> Albums.addReleased band releasedAlbum
    | AlbumReleasedUpdate(band, releasedAlbum) ->
        let album = releasedAlbum.Album

        Albums.removeReleased band album.Id state
        |> Albums.addReleased band releasedAlbum
    | AlbumReviewsReceived(band, releasedAlbum) ->
        let album = releasedAlbum.Album

        Albums.removeReleased band album.Id state
        |> Albums.addReleased band releasedAlbum
    | BalanceUpdated(account, Diff(_, balance)) ->
        Bank.setBalance account (Incoming(0m<dd>, balance)) state
    | BandFansChanged(band, Diff(_, fans)) -> Bands.changeFans band fans state
    | BandSwitchedGenre(band, Diff(_, genre)) ->
        Bands.changeGenre band genre state
    | CareerAccept(_, job) -> Career.set (Some job) state
    | CareerLeave _ -> Career.set None state
    | CareerPromoted(job, _) -> Career.set (Some job) state
    | CareerShiftPerformed _ -> state
    | CharacterAttributeChanged(character, attribute, Diff(_, amount)) ->
        Characters.setAttribute character attribute amount state
    | CharacterHealthDepleted _ -> state
    | CharacterHospitalized(_, (cityId, nodeId)) ->
        World.move cityId nodeId 0 state
    | CharacterMoodletsChanged(character, Diff(_, moodlets)) ->
        Characters.setMoodlets character moodlets state
    | ConcertScheduled(band, concert) ->
        Concerts.addScheduledConcert band concert state
    | ConcertUpdated(band, scheduledConcert) ->
        let concert = Concert.fromScheduled scheduledConcert

        Concerts.removeScheduledConcert band concert state
        |> Concerts.addScheduledConcert band scheduledConcert
    | ConcertFinished(band, pastConcert, _) ->
        let concert = Concert.fromPast pastConcert

        Concerts.removeScheduledConcert band concert state
        |> Concerts.addPastConcert band pastConcert
    | ConcertCancelled(band, pastConcert) ->
        let concert = Concert.fromPast pastConcert

        Concerts.removeScheduledConcert band concert state
        |> Concerts.addPastConcert band pastConcert
    | FlightBooked flight -> Flights.addBooking flight state
    | FlightUpdated flight -> Flights.change flight state
    | GameCreated state -> state
    | GenreMarketsUpdated genreMarkets -> Market.set genreMarkets state
    | ItemAddedToCharacterInventory item -> Inventory.addToCharacter item state
    | ItemChangedInCharacterInventory(Diff(prevItem, currItem)) ->
        Inventory.removeFromCharacter prevItem state
        |> Inventory.addToCharacter currItem
    | ItemRemovedFromCharacterInventory item ->
        Inventory.removeFromCharacter item state
    | ItemAddedToBandInventory(item, quantity) ->
        Inventory.addToBand item quantity state
    | ItemAddedToWorld(coords, item) -> World.add coords item state
    | ItemChangedInWorld(coords, Diff(prevItem, currItem)) ->
        World.remove coords prevItem state |> World.add coords currItem
    | ItemRemovedFromWorld(coords, item) -> World.remove coords item state
    | MerchPriceSet(band, merch, price) ->
        Merch.setPrice band.Id merch price state
    | MemberHired(band, character, currentMember, skills) ->
        let stateWithMember =
            Characters.add character state |> Bands.addMember band currentMember

        skills
        |> List.fold
            (fun currentState skill ->
                Skills.add currentMember.CharacterId skill currentState)
            stateWithMember
    | MemberFired(band, currentMember, pastMember) ->
        Bands.removeMember band currentMember state
        |> Bands.addPastMember band pastMember
    | NotificationScheduled(date, dayMoment, notification) ->
        Notifications.schedule date dayMoment notification state
    | NotificationShown _ -> state
    | MoneyTransferred(account, transaction) ->
        Bank.setBalance account transaction state
    | MoneyEarned(account, transaction) ->
        Bank.setBalance account transaction state
    | PlaceClosed _ -> state
    | PlayResult _ -> state
    | RelationshipChanged(npc, cityId, relationship) ->
        Relationships.changeForCharacterId npc.Id relationship state
        |> Relationships.changeForCityId npc.Id cityId relationship
        |> Characters.add npc
    | RentalAdded rental -> Rentals.add rental state
    | RentalKickedOut _ -> state
    | RentalExpired rental -> Rentals.remove rental state
    | RentalUpdated rental -> Rentals.remove rental state |> Rentals.add rental
    | SituationChanged situation ->
        Optic.set Lenses.State.situation_ situation state
    | SkillImproved(character, Diff(_, skill)) ->
        Skills.add character.Id skill state
    | SocialNetworkAccountCreated(socialNetworkKey, socialNetworkAccount) ->
        SocialNetworks.addAccount socialNetworkKey socialNetworkAccount state
    | SocialNetworkAccountFollowersChanged(socialNetworkKey,
                                           socialNetworkAccountId,
                                           Diff(_, followers)) ->
        SocialNetworks.updateFollowers
            socialNetworkKey
            socialNetworkAccountId
            followers
            state
    | SocialNetworkCurrentAccountChanged(socialNetworkKey,
                                         socialNetworkAccountId) ->
        SocialNetworks.switchAccount
            socialNetworkKey
            (SocialNetworkCurrentAccountStatus.Account socialNetworkAccountId)
            state
    | SocialNetworkPost(socialNetworkKey, post) ->
        SocialNetworks.addPost socialNetworkKey post state
    | SocialNetworkPostReposted(socialNetworkKey, socialNetworkPost, reposts) ->
        SocialNetworks.updateReposts
            socialNetworkKey
            socialNetworkPost
            reposts
            state
    | SongStarted(band, unfinishedSong) ->
        Songs.addUnfinished band unfinishedSong state
    | SongImproved(band, Diff(_, unfinishedSong)) ->
        Songs.addUnfinished band unfinishedSong state
    | SongFinished(band, finishedSong, finishDate) ->
        let song = Song.fromFinished finishedSong

        Songs.removeUnfinished band song.Id state
        |> Songs.addFinished band finishedSong finishDate
    | SongPracticed(band, finishedSong) ->
        Songs.updateFinished band finishedSong state
    | SongDiscarded(band, unfinishedSong) ->
        let song = Song.fromUnfinished unfinishedSong

        Songs.removeUnfinished band song.Id state
    | TimeAdvanced time -> Calendar.setTime time state
    | WorldEnterRoom(Diff(_, (cityId, placeId, romId))) ->
        World.move cityId placeId romId state
    | WorldMoveToPlace(Diff(_, (cityId, placeId, roomId))) ->
        World.move cityId placeId roomId state
    | WorldPeopleInCurrentRoomChanged people -> World.setPeople people state
    | Wait _ -> state

/// Applies multiple effects to the given initial state and returns the result.
let applyEffects initialState effects =
    (initialState, effects) ||> List.fold applyEffect
