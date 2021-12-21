module Cli.View.TextConstants

open Entities

/// Defines all verbs that we're using in the game that might have variations
/// depending on the gender or number. For example: for the verb have and a
/// character who's male it'd be he has, but if the character has the other
/// gender then it should be they have.
type VariableVerbs = | Have

/// Defines all the text constants available in the application. Since this
/// might change between each UI layer (might need custom styling, etc.) the
/// Game layer simply exports these as a type that gets evaluated in each UI.
/// All types must have the screen they belong to (if any) prepended to its name.
type TextConstant =
    | GameName
    | CommonYouAreIn of place: string
    | CommonChoiceSelection of selection: string
    | CommonMultiChoiceMoreChoices
    | CommonMultiChoiceInstructions
    | CommonNoUnfinishedSongs
    | CommonSkills
    | CommonCancel
    | CommonBack
    | CommonBackToMainMenu
    | CommonBackToMap
    | CommonBackToPhone
    | CommonBackToWorld
    | CommonCommandPrompt
    | CommonPressKeyToContinue
    | CommonSkillImproved of
        characterName: string *
        characterGender: Gender *
        skill: Skill *
        previousLevel: int *
        currentLevel: int
    | CommonStatusBar of
        date: Date *
        dayMoment: DayMoment *
        characterBalance: Amount *
        bandBalance: Amount
    | CommonInvalidLength
    | CommonInvalidCommand
    | CommandHelpDescription
    | CommandHelpEntry of string * Text
    | CommandDirectionDescription of direction: Direction
    | CommandLookDescription
    | CommandLookNoObjectsAround
    | CommandLookEnvironmentDescription of description: Text
    | CommandLookEntranceDescription of (Direction * Text) list
    | CommandLookObjectEntry of ObjectType * string list
    | CommandExitDescription
    | CommandMapDescription
    | CommandPhoneDescription
#if DEBUG
    | CommandDevDescription
#endif
    | MainMenuIncompatibleSavegame
    | MainMenuPrompt
    | MainMenuNewGame
    | MainMenuLoadGame
    | MainMenuExit
    | MainMenuSavegameNotAvailable
    | CharacterCreatorInitialPrompt
    | CharacterCreatorGenderPrompt
    | CharacterCreatorGenderMale
    | CharacterCreatorGenderFemale
    | CharacterCreatorGenderOther
    | CharacterCreatorAgePrompt
    | BandCreatorInitialPrompt
    | BandCreatorGenrePrompt
    | BandCreatorInstrumentPrompt
    | BandCreatorConfirmationPrompt of
        characterName: string *
        bandName: string *
        bandGenre: string *
        instrument: string
    | CreatorErrorCharacterNameTooShort
    | CreatorErrorCharacterNameTooLong
    | CreatorErrorCharacterAgeTooYoung
    | CreatorErrorCharacterAgeTooOld
    | CreatorErrorBandNameTooShort
    | CreatorErrorBandNameTooLong
    | WorldTitle
    | PhoneTitle
    | PhoneOptionBank
    | PhoneOptionStatistics
    | MapTitle
    | MapPrompt
    | MapOptionRehearsalRoom
    | MapOptionStudios
    | RehearsalSpaceLobbyName
    | RehearsalSpaceBarName
    | RehearsalSpaceRehearsalRoomName
    | RehearsalSpaceLobbyDescription
    | RehearsalSpaceBarDescription
    | RehearsalSpaceRehearsalRoomDescription
    | RehearsalRoomManageDescription
    | RehearsalRoomStatistics
    | RehearsalRoomInstrumentPlayDescription
    | ComposePrompt
    | ComposeSong
    | ComposeSongTitlePrompt
    | ComposeSongLengthPrompt
    | ComposeSongGenrePrompt
    | ComposeSongVocalStylePrompt
    | ComposeSongConfirmation of title: string
    | ComposeSongErrorNameTooShort
    | ComposeSongErrorNameTooLong
    | ComposeSongErrorLengthTooShort
    | ComposeSongErrorLengthTooLong
    | ComposeSongProgressBrainstorming
    | ComposeSongProgressTryingChords
    | ComposeSongProgressConfiguringReverb
    | ImproveSong
    | ImproveSongSelection
    | ImproveSongCanBeFurtherImproved of
        previousQuality: Quality *
        currentQuality: Quality
    | ImproveSongReachedMaxQuality
    | ImproveSongProgressAddingSomeMelodies
    | ImproveSongProgressPlayingFoosball
    | ImproveSongProgressModifyingChordsFromAnotherSong
    | FinishSong
    | FinishSongSelection
    | FinishSongFinished of name: string * quality: Quality
    | DiscardSong
    | DiscardSongSelection
    | DiscardSongDiscarded of name: string
    | PracticeSong
    | ManagementTitle
    | ManagementPrompt
    | ManagementHireMember
    | ManagementFireMember
    | ManagementMemberList
    | HireMemberRolePrompt
    | HireMemberSkillSummary of name: string * gender: Gender
    | HireMemberSkillLine of id: SkillId * level: int
    | HireMemberConfirmation of gender: Gender
    | HireMemberHired
    | HireMemberContinueConfirmation
    | FireMemberListItem of name: string * role: InstrumentType
    | FireMemberNoMembersToFire
    | FireMemberPrompt
    | FireMemberConfirmation of name: string
    | FireMemberConfirmed of name: string
    | MemberListCurrentTitle
    | MemberListCurrentMember of
        name: string *
        role: InstrumentType *
        since: Date
    | MemberListPastTitle
    | MemberListPastMember of
        name: string *
        role: InstrumentType *
        from: Date *
        until: Date
    | BankTitle
    | BankWelcome of characterBalance: Amount * bandBalance: Amount
    | BankPrompt
    | BankTransferToBand
    | BankTransferFromBand
    | BankTransferAmount of holder: BankAccountHolder
    | BankTransferSuccess of
        holder: BankAccountHolder *
        transaction: BankTransaction
    | BankTransferNotEnoughFunds
    | StudioCommonPromptReleaseAlbum of name: string
    | StudioCommonAlbumReleased of name: string
    | StudioTitle
    | StudioWelcomePrice of name: string * price: Amount
    | StudioPrompt
    | StudioStartRecord
    | StudioContinueRecord
    | StudioDiscardRecord
    | StudioCreateNoSongs
    | StudioCreateRecordName
    | StudioCreateTrackListPrompt
    | StudioConfirmRecordingPrompt of name: string * albumType: AlbumType
    | StudioCreateErrorNameTooShort
    | StudioCreateErrorNameTooLong
    | StudioCreateErrorNotEnoughMoney of studioBill: Amount
    | StudioCreateAlbumRecorded of albumName: string
    | StudioCreateProgressEatingSnacks
    | StudioCreateProgressRecordingWeirdSounds
    | StudioCreateProgressMovingKnobs
    | StudioContinueRecordPrompt
    | StudioContinueRecordActionPrompt
    | StudioContinueRecordActionPromptEditName
    | StudioContinueRecordActionPromptRelease
    | StudioContinueRecordAlbumRenamed of albumName: string
    | StatisticsTitle
    | StatisticsSectionPrompt
    | StatisticsSectionBand
    | StatisticsSectionAlbums
    | StatisticsBandName of name: string
    | StatisticsBandStartDate of date: Date
    | StatisticsBandFame of fame: int
    | StatisticsAlbumNoEntries
    | StatisticsAlbumName of name: string * albumType: AlbumType
    | StatisticsAlbumReleaseDate of date: Date
    | StatisticsAlbumStreams of streams: int
    | StatisticsAlbumRevenue of amount: Amount
    | PhonePrompt

/// Encapsulates text that can either be defined by a text constant, which is
/// resolved by the UI layer, or a string constant that is just passed from this
/// layer into the UI.
and Text =
    | TextConstant of TextConstant
    | Literal of string
